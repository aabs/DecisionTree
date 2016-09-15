
using System;
using Modd;

namespace unisuper
{
    class MemberMatcher
    {

        string ShouldAcceptMember(string _tfn, string _member_number, string _surname, string _dob, string _1st_character_of_given_name, string _payroll_number, string _data_source)
        {
            if (_member_number == "Correct")
            {
                if (_surname == "Correct")
                {
                    if (_dob == "Correct")
                    {
                        if (_tfn == "Correct")
                        {
                            if (_payroll_number == "Correct")
                            {
                                return "Matched";
                            }
                            if (_payroll_number == "Incorrect")
                            {
                                return "Matched with error";
                            }
                            if (_payroll_number == "Not supplied")
                            {
                                return "Matched";
                            }
                        }
                        if (_tfn == "Incorrect")
                        {
                            return "Matched with error";
                        }
                        if (_tfn == "Not supplied")
                        {
                            if (_payroll_number == "Correct")
                            {
                                return "Matched";
                            }
                            if (_payroll_number == "Incorrect")
                            {
                                return "Matched with error";
                            }
                            if (_payroll_number == "Not supplied")
                            {
                                return "Matched";
                            }
                        }
                    }
                    if (_dob == "Not supplied")
                    {
                        if (_1st_character_of_given_name == "Not supplied")
                        {
                            return "Unmatched";
                        }
                        if (_1st_character_of_given_name == "Incorrect")
                        {
                            return "Matched with error";
                        }
                        if (_1st_character_of_given_name == "Correct")
                        {
                            if (_payroll_number == "Correct")
                            {
                                return "Matched";
                            }
                            if (_payroll_number == "Incorrect")
                            {
                                return "Matched with error";
                            }
                            if (_payroll_number == "Not supplied")
                            {
                                return "Matched";
                            }
                        }
                    }
                    if (_dob == "Incorrect")
                    {
                        return "Matched with error";
                    }
                }
                if (_surname == "Incorrect")
                {
                    if (_dob == "Incorrect")
                    {
                        if (_1st_character_of_given_name == "Incorrect")
                        {
                            if (_tfn == "Incorrect")
                            {
                                if (_payroll_number == "Correct")
                                {
                                    return "Matched with error";
                                }
                                if (_payroll_number == "Incorrect")
                                {
                                    return "Unmatched";
                                }
                                if (_payroll_number == "Not supplied")
                                {
                                    return "Unmatched";
                                }
                            }
                            if (_tfn == "Correct")
                            {
                                return "Matched with error";
                            }
                            if (_tfn == "Not supplied")
                            {
                                if (_payroll_number == "Correct")
                                {
                                    return "Matched with error";
                                }
                                if (_payroll_number == "Incorrect")
                                {
                                    return "Unmatched";
                                }
                                if (_payroll_number == "Not supplied")
                                {
                                    return "Unmatched";
                                }
                            }
                        }
                        if (_1st_character_of_given_name == "Correct")
                        {
                            return "Matched with error";
                        }
                        if (_1st_character_of_given_name == "Not supplied")
                        {
                            return "Unmatched";
                        }
                    }
                    if (_dob == "Correct")
                    {
                        return "Matched with error";
                    }
                    if (_dob == "Not supplied")
                    {
                        return "Matched with error";
                    }
                }
                if (_surname == "Not supplied")
                {
                    return "Matched";
                }
            }
            if (_member_number == "Incorrect")
            {
                if (_tfn == "Correct")
                {
                    if (_surname == "Incorrect")
                    {
                        if (_dob == "Incorrect")
                        {
                            if (_1st_character_of_given_name == "Correct")
                            {
                                return "Matched with error";
                            }
                            if (_1st_character_of_given_name == "Not supplied")
                            {
                                return "Unmatched";
                            }
                            if (_1st_character_of_given_name == "Incorrect")
                            {
                                if (_payroll_number == "Correct")
                                {
                                    return "Matched with error";
                                }
                                if (_payroll_number == "Incorrect")
                                {
                                    return "Unmatched";
                                }
                                if (_payroll_number == "Not supplied")
                                {
                                    return "Unmatched";
                                }
                            }
                        }
                        if (_dob == "Correct")
                        {
                            return "Matched with error";
                        }
                        if (_dob == "Not supplied")
                        {
                            return "Unmatched";
                        }
                    }
                    if (_surname == "Correct")
                    {
                        return "Matched with error";
                    }
                    if (_surname == "Not supplied")
                    {
                        return "Unmatched";
                    }
                }
                if (_tfn == "Incorrect")
                {
                    if (_surname == "Correct")
                    {
                        if (_dob == "Correct")
                        {
                            if (_1st_character_of_given_name == "Correct")
                            {
                                return "Matched with error";
                            }
                            if (_1st_character_of_given_name == "Incorrect")
                            {
                                return "Unmatched";
                            }
                            if (_1st_character_of_given_name == "Not supplied")
                            {
                                return "Unmatched";
                            }
                        }
                        if (_dob == "Incorrect")
                        {
                            return "Unmatched";
                        }
                        if (_dob == "Not supplied")
                        {
                            return "Unmatched";
                        }
                    }
                    if (_surname == "Incorrect")
                    {
                        return "Unmatched";
                    }
                    if (_surname == "Not supplied")
                    {
                        return "Unmatched";
                    }
                }
                if (_tfn == "Not supplied")
                {
                    if (_surname == "Correct")
                    {
                        if (_dob == "Correct")
                        {
                            if (_1st_character_of_given_name == "Correct")
                            {
                                return "Matched with error";
                            }
                            if (_1st_character_of_given_name == "Incorrect")
                            {
                                return "Unmatched";
                            }
                            if (_1st_character_of_given_name == "Not supplied")
                            {
                                return "Unmatched";
                            }
                        }
                        if (_dob == "Incorrect")
                        {
                            return "Unmatched";
                        }
                        if (_dob == "Not supplied")
                        {
                            return "Unmatched";
                        }
                    }
                    if (_surname == "Incorrect")
                    {
                        return "Unmatched";
                    }
                    if (_surname == "Not supplied")
                    {
                        return "Unmatched";
                    }
                }
            }
            if (_member_number == "Not supplied")
            {
                if (_tfn == "Correct")
                {
                    if (_surname == "Correct")
                    {
                        if (_dob == "Incorrect")
                        {
                            return "Matched with error";
                        }
                        if (_dob == "Correct")
                        {
                            if (_payroll_number == "Correct")
                            {
                                return "Matched";
                            }
                            if (_payroll_number == "Incorrect")
                            {
                                return "Matched with error";
                            }
                            if (_payroll_number == "Not supplied")
                            {
                                return "Matched";
                            }
                        }
                        if (_dob == "Not supplied")
                        {
                            return "Unmatched";
                        }
                    }
                    if (_surname == "Incorrect")
                    {
                        if (_dob == "Incorrect")
                        {
                            if (_1st_character_of_given_name == "Correct")
                            {
                                return "Matched with error";
                            }
                            if (_1st_character_of_given_name == "Not supplied")
                            {
                                return "Unmatched";
                            }
                            if (_1st_character_of_given_name == "Incorrect")
                            {
                                if (_payroll_number == "Correct")
                                {
                                    return "Matched with error";
                                }
                                if (_payroll_number == "Incorrect")
                                {
                                    return "Unmatched";
                                }
                                if (_payroll_number == "Not supplied")
                                {
                                    return "Unmatched";
                                }
                            }
                        }
                        if (_dob == "Correct")
                        {
                            return "Matched with error";
                        }
                        if (_dob == "Not supplied")
                        {
                            return "Unmatched";
                        }
                    }
                    if (_surname == "Not supplied")
                    {
                        return "Unmatched";
                    }
                }
                if (_tfn == "Not supplied")
                {
                    if (_dob == "Correct")
                    {
                        if (_surname == "Correct")
                        {
                            if (_1st_character_of_given_name == "Correct")
                            {
                                if (_payroll_number == "Correct")
                                {
                                    return "Matched";
                                }
                                if (_payroll_number == "Incorrect")
                                {
                                    return "Matched with error";
                                }
                                if (_payroll_number == "Not supplied")
                                {
                                    return "Matched";
                                }
                            }
                            if (_1st_character_of_given_name == "Incorrect")
                            {
                                return "Unmatched";
                            }
                            if (_1st_character_of_given_name == "Not supplied")
                            {
                                return "Unmatched";
                            }
                        }
                        if (_surname == "Incorrect")
                        {
                            return "Unmatched";
                        }
                        if (_surname == "Not supplied")
                        {
                            return "Unmatched";
                        }
                    }
                    if (_dob == "Not supplied")
                    {
                        if (_payroll_number == "Correct")
                        {
                            if (_surname == "Correct")
                            {
                                if (_1st_character_of_given_name == "Correct")
                                {
                                    return "Matched";
                                }
                                if (_1st_character_of_given_name == "Incorrect")
                                {
                                    return "Matched with error";
                                }
                                if (_1st_character_of_given_name == "Not supplied")
                                {
                                    return "Unmatched";
                                }
                            }
                            if (_surname == "Incorrect")
                            {
                                return "Matched with error";
                            }
                            if (_surname == "Not supplied")
                            {
                                return "Unmatched";
                            }
                        }
                        if (_payroll_number == "Incorrect")
                        {
                            if (_surname == "Correct")
                            {
                                if (_1st_character_of_given_name == "Correct")
                                {
                                    return "Matched with error";
                                }
                                if (_1st_character_of_given_name == "Incorrect")
                                {
                                    return "Unmatched";
                                }
                                if (_1st_character_of_given_name == "Not supplied")
                                {
                                    return "Unmatched";
                                }
                            }
                            if (_surname == "Incorrect")
                            {
                                return "Unmatched";
                            }
                            if (_surname == "Not supplied")
                            {
                                return "Unmatched";
                            }
                        }
                        if (_payroll_number == "Not supplied")
                        {
                            return "Unmatched";
                        }
                    }
                    if (_dob == "Incorrect")
                    {
                        return "Unmatched";
                    }
                }
                if (_tfn == "Incorrect")
                {
                    if (_surname == "Correct")
                    {
                        if (_dob == "Correct")
                        {
                            if (_1st_character_of_given_name == "Correct")
                            {
                                return "Matched with error";
                            }
                            if (_1st_character_of_given_name == "Incorrect")
                            {
                                return "Unmatched";
                            }
                            if (_1st_character_of_given_name == "Not supplied")
                            {
                                return "Unmatched";
                            }
                        }
                        if (_dob == "Incorrect")
                        {
                            return "Unmatched";
                        }
                        if (_dob == "Not supplied")
                        {
                            return "Unmatched";
                        }
                    }
                    if (_surname == "Incorrect")
                    {
                        return "Unmatched";
                    }
                    if (_surname == "Not supplied")
                    {
                        return "Unmatched";
                    }
                }
            }
        } // end function
    } // end class
} // end namespace

