//----------------------------------------------------------------------------
//
// Copyright © 2019 Dipl.-Ing. (BA) Steffen Liersch
// All rights reserved.
//
// Steffen Liersch
// Robert-Schumann-Straße 1
// 08289 Schneeberg
// Germany
//
// Phone: +49-3772-38 28 08
// E-Mail: S.Liersch@gmx.de
//
//----------------------------------------------------------------------------

/*
 * This project demonstrates an issue with function JsValue.ToObject.
 * It causes a StackOverflowException for objects with circular references.
 * 
 * Jint 2.11.10        : fails
 * Jint 3.0.0-beta-1353: fails
 */

using System;
using Jint;
using Jint.Native;
using Jint.Runtime.Interop;

namespace JintDemo
{
  static class Program
  {
    static void Test(string message, object value) { Console.WriteLine(message); }

    static void Main(string[] args)
    {
      var engine=new Engine();

      engine.Global.FastAddProperty("global", engine.Global, true, true, true);
      engine.Global.FastAddProperty("test", new DelegateWrapper(engine, (Action<string, object>)Test), true, true, true);

      //JsValue v=engine.Global;
      //v.ToObject(); // Causes a StackOverflowException

      engine.Execute(@"
var demo={};
demo.value=1;

test('Test 1', demo.value===1);
test('Test 2', demo.value);

demo.demo=demo;
test('Test 3', demo); // Causes a StackOverflowException

test('Test 4', global); // Causes a StackOverflowException
");

      Console.ReadKey(true);
    }
  }
}