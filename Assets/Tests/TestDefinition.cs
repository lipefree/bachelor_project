using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestDefinitions
{
    [Test]
    public void KindOfTest1()
    {
        char c = '3';
        var lexer = new Lexer();
        var tokens = lexer.kindOf(c);

        var check = tokens.Contains(TokenType.Identifier) && tokens.Contains(TokenType.IntLit);
        Assert.IsTrue(check, "Tokens should be of type identifier AND intLit");
    }

    [Test]
    public void KindOfTest2()
    {
        char c = '%';
        var lexer = new Lexer();
        var tokens = lexer.kindOf(c);

        var check = tokens.Contains(TokenType.NotDefined);
        Assert.IsTrue(check, "Tokens should be of type notDefined");
    }
    [Test]
    public void KindOfTest3()
    {
        char c = ' ';
        var lexer = new Lexer();
        var tokens = lexer.kindOf(c);

        var check = tokens.Contains(TokenType.Space);
        Assert.IsTrue(check, "Tokens should be of type space");
        Assert.IsTrue(tokens.Count == 1, "To many type was match, we should only get spaces");
    }

    [Test]
    public void lmpTest1()
    {
        var def = "test123";
        var lexer = new Lexer();
        var (tokenType, newDef) = lexer.lmp(def);

        Assert.IsTrue(tokenType == TokenType.Identifier, "Tokens should be of type Identifier");
        Assert.IsTrue(newDef.Equals(def), "Newdef does not correspond to def");
    }

    [Test]
    public void lmpTest2()
    {
        var def = "123test";
        var check = "123";
        var lexer = new Lexer();
        var (tokenType, newDef) = lexer.lmp(def);

        Debug.Log(newDef);
        Assert.IsTrue(tokenType == TokenType.IntLit, "Tokens should be of type intLit");
        Assert.IsTrue(newDef.Equals(check), "Newdef does not correspond to 123");
    }
}