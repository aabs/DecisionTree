using Modd;
using Modd.Metadata;
using Nortal.Utilities.Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionDiagram.Metadata
{
    /// <summary>
    /// Fluent builder for a metadata structure.
    /// </summary>
    /// <remarks>
    /// to support the generation of metadata from inspection of a data file
    /// </remarks>
    public class MetadataBuilder
    {
        readonly string location;
        private string columnHeading;
        private bool inspected;
        private char delimiter = ',';
        public DataSet Samples { get; set; }
        public MetadataBuilder(string location)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location));
            if (!File.Exists(location))
            {
                throw new FileNotFoundException(location);
            }
            this.location = location;
            InspectDataFileForMetadata();
            inspected = true;
        }

        public MetadataBuilder WithOutcomeColumn(string columnHeading)
        {
            if (columnHeading == null)
                throw new ArgumentNullException(nameof(columnHeading));
            this.columnHeading = columnHeading;
            if (columns != null && columns.Count() > 0)
            {
                columns.Where(c => c.Heading.Equals(columnHeading))
                    .Foreach(c => c.IsOutcomeColumn = true);
            }
            return this;
        }


        public MetadataBuilder WithColumn(int columnIndex, string heading, params string[] values)
        {
            StartColumn(columnIndex, heading);
            foreach (var v in values)
            {
                DetectValue(v);
            }
            EndColumn();
            return this;
        }

        public MetadataBuilder IgnoringField(string columnHeading)
        {
            throw new NotImplementedException();
        }

        public MetadataBuilder WithFieldDelimiter(char delimiter)
        {
            this.delimiter = delimiter;
            return this;
        }

        public DecisionMetadata Build()
        {
            if (!inspected)
            {
                InspectDataFileForMetadata();
            }
            //throw new NotImplementedException();
            var outcomes = (from c in columns
                            where c.IsOutcomeColumn
                            select c).FirstOrDefault();
            if (outcomes == null)
            {
                throw new DecisionException("no decision outcome column name specified");
            }
            return new DecisionMetadata
            {
                Attributes = (from c in columns
                              where !c.IsOutcomeColumn
                              select new Modd.Metadata.Attribute
                              (
                                  name: c.Heading,
                                  vals: c.Values
                                  .OrderBy(v => v)
                                  .Select(v=>v)
                                  .ToArray()
                              )).ToList(),
                Outcomes = new DecisionOutcomes
                {
                    OutcomeColumnNameInSampleData = outcomes.Heading,
                    OutcomeType = "Enumerated",
                    Values = outcomes.Values
                    .OrderBy(x => x)
                    .Select((v,i) => new DecisionOutcome { Id = i, Name = v}).ToList()
                },
                SampleDataLocation = location                
            };
        }

        private void InspectDataFileForMetadata()
        {
            var csvString = File.ReadAllText(this.location);
            var data = CsvParser.Parse(csvString, new CsvSettings { FieldDelimiter = this.delimiter, RowDelimiter = "\n" });

            var headings = data[0];
            for (int columnIndex = 0; columnIndex < headings.Length; columnIndex++)
            {
                StartColumn(columnIndex, headings[columnIndex]);
                for (int rowIndex = 1; rowIndex < data.Length; rowIndex++)
                {
                    DetectValue(data[rowIndex][columnIndex]);
                }
                EndColumn();
            }
        }

        void DetectValue(string value)
        {
            if (CurrentColumn != null)
            {
                CurrentColumn.Values.Add(value);
            }
        }

        private void EndColumn()
        {
            if (CurrentColumn != null)
            {
                columns.Add(CurrentColumn);
                CurrentColumn = null;
            }
        }

        private List<ColumnMetadata> columns = new List<ColumnMetadata>();

        public class ColumnMetadata
        {
            public bool IsOutcomeColumn { get; set; }
            public int ColumnIndex { get; set; }
            public string Heading { get; set; }
            public HashSet<string> Values { get; set; }
        }
        public ColumnMetadata CurrentColumn { get; set; }
        private void StartColumn(int columnIndex, string heading)
        {
            CurrentColumn = new ColumnMetadata
            {
                ColumnIndex = columnIndex,
                Heading = heading,
                Values = new HashSet<string>()
            };
        }
    }
}
