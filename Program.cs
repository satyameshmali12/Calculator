using System;
using System.Collections;
using System.Collections.Generic;

public class Demo
{
    public static void Main(string[] args)
    {

        // demo
        Calculator calci = new Calculator();
        string numeric1 = "100+100"; 
        float value1 = calci.getSolve(numeric1);
        System.Console.WriteLine($"{numeric1} :- {value1}");

        string numeric2 = "34+34*2*3+2+34*3*(34)";
        float value2 = calci.getSolve(numeric2);
        System.Console.WriteLine($"{numeric2} :- {value2}");

    }

    
}