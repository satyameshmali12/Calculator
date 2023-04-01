// The steps involve in calculation
// 1> Numeric wrapped with the braces i.e (numeric)                                     |    cleaning of the
// 2> Then remove all the white spaces from the numeric e.g  (34+  34+3) -> (34+34+3)   |    unnecessary stuff
// 3> removing the unnecessary brackets from the numeric e.g (34+32+58)) -> (34+32+58)  |    in the numeric
// 4> Adding the brackets on the mulitplication and dividing side                       |

// 5> And the last and the main step calculating value e.g (34+2) -> 36                 |    computation of the actual numeric 
 
 using System;
using System.Collections;


public class Calculator
{   
    // Use this function to solve the numeric   
    public float getSolve(string numeric)
    {
        numeric = "("+numeric+")";
        numeric = removeAllWhiteSpaces(numeric);
        numeric = removeUnNecBrc(numeric);
        numeric = getNumericReady(numeric);
        return solveNumerical(numeric);
    }

    public float solveNumerical(string numeric)
    {
        numeric = numeric.Remove(0, 1).Remove(numeric.Length - 2, 1);

        float sum = 0;

        string number = "";

        ArrayList operators = new ArrayList() { "+", "-", "*", "/", "%" };
        ArrayList data = new ArrayList();

        string? currentOpera = "";
        
        for (var i = 0; i < numeric.Length; i++)
        {

            string sym = numeric[i].ToString();

            if (sym == "(")
            {
                int startIndex = i;
                string restNumeric = numeric.Substring(startIndex, numeric.Length - startIndex);
                int length = getNextCloseBracketIndex(restNumeric)+1;
                string newNumeric = numeric.Substring(startIndex, length);
                i += newNumeric.Length-1;
                float newVal = solveNumerical(newNumeric);
                currentOpera = (!operators.Contains(currentOpera)) ? "+" : currentOpera;
                sum = resolveValue(sum, newVal, currentOpera);
                continue;
            }

            if (!operators.Contains(sym))
            {
                number += sym;
            }

            if (operators.Contains(sym) || i == numeric.Length - 1)
            {
                currentOpera = (!operators.Contains(currentOpera)) ? "+" : currentOpera;
                number = (number=="")?(currentOpera=="+" || currentOpera=="-")?"0":"1":number;
                sum = resolveValue(sum, float.Parse(number), currentOpera);
                number = "";
                currentOpera = sym;
            }

        }

        return sum;
    }

    string removeUnNecBrc(string numeric)
    {
        Dictionary<string, string> getNumData(string sym, int i, bool tF, string brcIndex)
        {
            return new Dictionary<string, string>() { { "Sym", sym }, { "Index", i.ToString() }, { "Is_Compataible", "false" }, { "Brc_Index", brcIndex.ToString() } };

        }
        ArrayList numbers = new ArrayList() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
        ArrayList unneBrac = new ArrayList();
        int brcIndex = 0;
        for (var i = 0; i < numeric.Length; i++)
        {
            var sym = numeric[i].ToString();
            if (sym == "(")
            {
                brcIndex++;
                unneBrac.Add
                (
                    getNumData(sym, i, false, brcIndex.ToString())
                );
            }
            else if (sym == ")")
            {
                bool isToAdd = false;
                if (i != numeric.Length - 1 && numbers.Contains(numeric[i + 1].ToString()))
                {
                    isToAdd = true;
                }
                else
                {
                    bool hasEnter = false;
                    foreach (Dictionary<string, string> data in unneBrac)
                    {
                        if (data["Sym"] == "(" && data["Brc_Index"] == brcIndex.ToString())
                        {
                            hasEnter = true;
                            data["Is_Compataible"] = "true";
                            brcIndex--;
                            isToAdd = false;
                            break;
                        }
                    }
                    isToAdd = !hasEnter;
                }
                if (isToAdd)
                {
                    unneBrac.Add
                    (
                        getNumData(sym, i, false, "Anomymous")
                    );
                }
            }
        }

        unneBrac.Reverse();
        foreach (Dictionary<string, string> data in unneBrac)
        {
            if (!bool.Parse(data["Is_Compataible"]))
            {
                numeric = numeric.Remove(int.Parse(data["Index"]));
            }
        }

        return numeric;

    }

    string removeAllWhiteSpaces(string numeric)
    {
        ArrayList spIndexes = new ArrayList();
        for (var i = 0; i < numeric.Length; i++)
        {
            string sym = numeric[i].ToString();
            if (sym == " ")
            {
                spIndexes.Add(spIndexes);
            }
        }
        spIndexes.Reverse();
        foreach (int index in spIndexes)
        {
            numeric.Remove(index);
        }
        return numeric;
    }

   
    int getNextCloseBracketIndex(string value)
    {
        int opBracCount = 0;
        for (var i = 0; i < value.Length; i++)
        {
            string sym = value[i].ToString();

            opBracCount = (sym == "(") ? ++opBracCount : (sym == ")") ? --opBracCount : opBracCount;

            if (opBracCount == 0 && sym == ")")
            {
                return i;
            }
        }
        return opBracCount;
    }

    public string getNumericReady(string numeric)
    {
        ArrayList allOpp = new ArrayList() { "+", "-", "*", "/" };
        ArrayList exeOpp = new ArrayList() { "/", "*" };
        ArrayList newBrcData = new ArrayList();

        //34*2
        bool isToReRollIndex = true;
        bool isAddingNumFounded = false;
        int startIndex = 0;

        string lastOpe = "";
        string currOpe = "";

        for (var i = 0; i < numeric.Length; i++)
        {
            bool isLastIte = (i == numeric.Length - 1);
            if (isToReRollIndex)
            {
                startIndex = i;
                isToReRollIndex = false;
            }
            string sym = numeric[i].ToString();

            if (allOpp.Contains(sym) || isLastIte)
            {
                if(currOpe!="")
                {
                    lastOpe = currOpe;
                }
                currOpe = sym;

                if (isAddingNumFounded && lastOpe!=currOpe)
                {
                    // System.Console.WriteLine("Hello world");
                    newBrcData.Add(new Dictionary<string, string>() { { "Index", startIndex.ToString() }, { "Sym", "(" } });
                    newBrcData.Add(new Dictionary<string, string>() { { "Index", (i + ((isLastIte) ? 1 : 0)).ToString() }, { "Sym", ")" } });
                    isAddingNumFounded = false;
                }
                if (exeOpp.Contains(sym))
                {
                    isAddingNumFounded = true;
                }
                else
                {
                    isToReRollIndex = true;
                }

            }
        }
        ArrayList arrangedBrcData = new ArrayList();
        int orgLength = newBrcData.Count;
        while (arrangedBrcData.Count != orgLength)
        {
            Dictionary<string,string>? addData = null;
            foreach (Dictionary<string, string> data in newBrcData)
            {
                if(addData==null)
                {
                    addData = data;
                }
                else if(int.Parse(addData["Index"])<int.Parse(data["Index"]))
                {
                    addData = data;
                }
            }

            arrangedBrcData.Add(addData);
            newBrcData.Remove(addData);
        }

        foreach (Dictionary<string,string> data in arrangedBrcData)
        {
            numeric = numeric.Insert(int.Parse(data["Index"]),data["Sym"]);
        }


        return numeric;


    }

    float resolveValue(float currentValue, float newValue, string oper)
    {
        Dictionary<string, float> data = new Dictionary<string, float>()
        {
                {"+",currentValue+newValue},
                {"-",currentValue-newValue},
                {"*",currentValue*newValue},
                {"/",currentValue/newValue},
                {"%",currentValue%newValue}
        };

        return data[oper];

    }
    
}



// Copyright by AJTA's
// For more information visit www.github.com/satyameshmali12
// For contacting us :- satyameshmalimern123@gmail.com