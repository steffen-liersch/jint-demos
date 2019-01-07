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
 * This project demonstrates an issue with the const-clause.
 * Constants keep available after leaving the scope.
 * 
 * Jint 2.11.10        : succeeds
 * Jint 3.0.0-beta-1353: fails
 */

#define JINT3

using System;
using Jint;
using Jint.Native;
using Jint.Runtime.Environments;
using Jint.Runtime.Interop;

namespace JintDemo
{
  static class Program
  {
    static void Main(string[] args)
    {
      var engine=new Engine();

      engine.Global.FastAddProperty("assert", new DelegateWrapper(engine, (Action<bool>)Assert), true, true, true);

      Execute(engine, "assert(typeof value==='undefined');");
      Execute(engine, "var value=1;");
      Execute(engine, "assert(typeof value==='undefined');");
      Execute(engine, "var value=1;");
      Execute(engine, "assert(typeof value==='undefined');");
      Execute(engine, "const value=1;");
      Execute(engine, "assert(typeof value==='undefined');"); // Fails
      Execute(engine, "var value=1;");
      Execute(engine, "assert(typeof value==='undefined');"); // Fails

      Console.ReadKey(true);
    }

    static void Execute(Engine engine, string source)
    {
      var envRec=new DeclarativeEnvironmentRecord(engine);

#if JINT3
      var lexEnv=new LexicalEnvironment(engine, envRec, engine.GlobalEnvironment);
#else
      var lexEnv=new LexicalEnvironment(envRec, engine.GlobalEnvironment);
#endif

      engine.EnterExecutionContext(lexEnv, lexEnv, JsValue.Null);
      try
      {
        Console.WriteLine("// "+source);
        engine.Execute(source);
        Console.WriteLine();
      }
      finally
      {
        engine.LeaveExecutionContext();
      }
    }

    static void Assert(bool condition) { Console.WriteLine(condition ? "[OK]" : "[FAILED]"); }
  }
}