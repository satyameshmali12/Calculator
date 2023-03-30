using System;
using System.Collections;
using System.Collections.Generic;

public class MyClass
{
    public static void Main(string[] args)
    {
        MyClass tCls = new MyClass();
        System.Console.WriteLine(tCls.getNumericReady("(34+34)*10"));

    }

    
    public float getSolve(string numeric)
    {
        numeric = "("+numeric+")";
        // numeric = removeAllWhiteSpaces(numeric);
        // numeric = removeUnNecBrc(numeric);
        numeric = getNumericReady(numeric);
        System.Console.WriteLine(numeric);
        return solveNumerical(numeric);
    }


    public string removeUnNecBrc(string numeric)
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

    public string removeAllWhiteSpaces(string numeric)
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

    public string getNumericReady(string numeric)
    {        
        
        ArrayList brcIndexes = new ArrayList();
        bool isNumFou = false;

        bool isArOpWasThere = true;
        ArrayList addOpp = new ArrayList() { "*", "/" };
        ArrayList allOpp = new ArrayList() { "*", "/", "+", "-", "%" };

        int numStart = 0;
        for (var i = 0; i < numeric.Length; i++)
        {

            bool isLastIte = i == numeric.Length - 1;
            string sym = numeric[i].ToString();

            if (isArOpWasThere)
            {
                numStart = i;
                isArOpWasThere = false;
            }
            else if (allOpp.Contains(sym) || isLastIte)
            {
                if (isNumFou)
                {
                    brcIndexes.Add(new Dictionary<string, string>() { { "Sym", "(" }, { "Index", numStart.ToString() } });
                    brcIndexes.Add(new Dictionary<string, string>() { { "Sym", ")" }, { "Index", (i + ((isLastIte) ? 1 : 0)).ToString() } });
                    isNumFou = false;
                }

                if (!addOpp.Contains(sym))
                {
                    isArOpWasThere = true;
                }
                else
                {
                    isNumFou = true;
                }
            }
        }

        ArrayList arrangedData = new ArrayList();
        int orgCount = brcIndexes.Count;
        Dictionary<string, string>? brcData = null;
        while (arrangedData.Count != orgCount)
        {
            foreach (Dictionary<string, string> data in brcIndexes)
            {
                if (brcData == null)
                {
                    brcData = data;
                }
                else if (int.Parse(data["Index"]) > int.Parse(brcData["Index"]))
                {
                    brcData = data;
                }
            }
            brcIndexes.Remove(brcData);
            arrangedData.Add(brcData);
            brcData = null;
        }

        foreach (Dictionary<string, string> data in arrangedData)
        {
            numeric = numeric.Insert(int.Parse(data["Index"]), data["Sym"]);
        }

        return numeric;

    }

    public int getNextCloseBracketIndex(string value)
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

    public float solveNumerical(string numeric)
    {
        numeric = numeric.Remove(0, 1).Remove(numeric.Length - 2, 1);

        float sum = 0;

        string number = "";

        ArrayList operators = new ArrayList() { "+", "-", "*", "/", "%" };
        ArrayList data = new ArrayList();

        string? currentOpera = null;

        for (var i = 0; i < numeric.Length; i++)
        {

            string sym = numeric[i].ToString();

            if (sym == "(")
            {
                int startIndex = i;
                string restNumeric = numeric.Substring(startIndex, numeric.Length - startIndex);
                int length = getNextCloseBracketIndex(restNumeric) + 1;
                string newNumeric = numeric.Substring(startIndex, length);
                i += newNumeric.Length;
                float newVal = solveNumerical(newNumeric);
                sum += newVal;
                continue;
            }

            if (!operators.Contains(sym))
            {
                number += sym;
            }

            if (operators.Contains(sym) || i == numeric.Length - 1)
            {
                currentOpera = (!operators.Contains(currentOpera)) ? "+" : currentOpera;
                sum = resolveValue(sum, float.Parse(number), currentOpera);
                number = "";
                currentOpera = sym;
            }
        }

        return sum;
    }


    public float resolveValue(float currentValue, float newValue, string oper)
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