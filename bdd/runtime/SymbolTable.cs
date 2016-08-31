using Modd.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modd
{
    public class SymbolTableEntry
    {
        public int Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// The type of the variable being tested
        /// </summary>
        public Type AttributeType { get; set; }
        /// <summary>
        /// An enumeration of the possible instances of this attribute, 
        /// or a pre-defined set of subsets of the possible values.
        /// </summary>
        public List<AttributePermissibleValue> PermittedValues { get; set; }
    }

    /// <summary>
    /// An instance of a classification of the members of a set.
    /// </summary>
    /// <remarks>
    /// To explain further.  If an attribute is to be classified in various ways, 
    /// (i.e. age might be divided into bands, booleans into true or false, enums into specific values)
    /// then this class is used to represent one of the instances of the classification.
    /// </remarks>
    public class AttributePermissibleValue
    {
        // The name given to the classification set
        public string ClassName { get; set; }

        /// <summary>
        /// The expected value (if that makes sense)
        /// </summary>
        public object Value { get; set; }
        // perhaps there will be a function here to allow a sample value to be tested to see whether it belongs to the class/set
    }

    public class SymbolTable
    {
        private static int nextId = 0;

        Dictionary<string, SymbolTableEntry> symbols = new Dictionary<string, SymbolTableEntry>();

        public SymbolTable(DecisionMetadata d)
        {
            this.DecisionMetadata = d;
        }

        public SymbolTable()
        {
        }

        public DecisionMetadata DecisionMetadata { get; internal set; }

        public Dictionary<string, SymbolTableEntry> Symbols
        {
            get
            {
                return symbols;
            }

            set
            {
                symbols = value;
            }
        }

        public int DeclareBooleanVariable(string variableName)
        {
            if (Symbols.ContainsKey(variableName))
            {
                throw new DecisionException("symbol already in use");
            }
            var newEntry = new SymbolTableEntry
            {
                Id = nextId++,
                Name = variableName,
                AttributeType = typeof(bool),
                PermittedValues = new List<AttributePermissibleValue> {
                        new AttributePermissibleValue {ClassName ="true", Value = true },
                        new AttributePermissibleValue {ClassName ="false", Value = false }
                    }
            };
            Symbols[newEntry.Name] = newEntry;
            return newEntry.Id;
        }
        public int DeclareMaybeBooleanVariable(string variableName)
        {
            if (Symbols.ContainsKey(variableName))
            {
                throw new DecisionException("symbol already in use");
            }
            var newEntry = new SymbolTableEntry
            {
                Id = nextId++,
                Name = variableName,
                AttributeType = typeof(int),
                PermittedValues = new List<AttributePermissibleValue> {
                        new AttributePermissibleValue {ClassName ="missing", Value = 0 },
                        new AttributePermissibleValue {ClassName ="true", Value = 1 },
                        new AttributePermissibleValue {ClassName ="false", Value = 2 }
                    }
            };
            Symbols[newEntry.Name] = newEntry;
            return newEntry.Id;
        }

        internal int DeclareEnumeratedVariable(string variableName, List<PossibleValue> vals)
        {
            if (Symbols.ContainsKey(variableName))
            {
                throw new DecisionException("symbol already in use");
            }
            var newEntry = new SymbolTableEntry
            {
                Id = nextId++,
                Name = variableName,
                AttributeType = typeof(int),
                PermittedValues = vals.Select(p => new AttributePermissibleValue { ClassName = p.Value, Value = p.Value }).ToList()
            };
            Symbols[newEntry.Name] = newEntry;
            return newEntry.Id;
        }

        public int? GetSymbolId(string name)
        {
            return GetEntry(name)?.Id;
        }

        public SymbolTableEntry GetEntry(string name)
        {
            if (!Symbols.ContainsKey(name))
            {
                return null;
            }
            return Symbols[name];
        }
    }

    public class SymbolTableBuilder
    {
        DecisionMetadata d;
        Dictionary<string, string[]> symbols = new Dictionary<string, string[]>();
        public SymbolTableBuilder WithSymbol(string name, params string[] vals)
        {
            symbols[name] = vals;
            return this;
        }

        public SymbolTableBuilder WithMetadata(DecisionMetadata d)
        {
            this.d = d;
            return this;
        }

        public SymbolTable Build()
        {
            int idDispenser = 1;
            var result = new SymbolTable(d);
            if (symbols.Count >= 0)
            {
                foreach (var s in symbols)
                {
                    var ste = new SymbolTableEntry
                    {
                        AttributeType = typeof(int),
                        Id = idDispenser++,
                        Name = s.Key,
                        PermittedValues = s.Value.Select(v => new AttributePermissibleValue { Value = v, ClassName = v }).ToList()
                    };
                    result.Symbols[s.Key] = ste;
                }
            }
            else
            {
                result.DeclareMaybeBooleanVariable("Surname");
                result.DeclareMaybeBooleanVariable("First Name");
                result.DeclareMaybeBooleanVariable("DOB");
            }
            return result;
        }
    }
}
