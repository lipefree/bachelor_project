using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML.Probabilistic.Models;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
// using Microsoft.ML.Probabilistic.Models;
// using Microsoft.ML.Probabilistic;

public class TestDefinitions
{   
    InferenceEngine engine = new InferenceEngine();  
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

    [Test]
    public void parseTest1()
    {
        var def = "123test";
        var lexer = new Lexer();
        var result = lexer.parse(def);

        Assert.IsTrue(result[0].Item1 == TokenType.IntLit, "First token should be of type intLit");
        Assert.IsTrue(result[0].Item2.Equals("123"), "First token should contain 123");
        Assert.IsTrue(result[1].Item1 == TokenType.Identifier, "Second token should be of type identifier");
        Assert.IsTrue(result[1].Item2.Equals("test"), "Second token should contain test");
        Assert.IsTrue(result[2].Item1 == TokenType.EDF, "Last token should be of type EDF");
        Assert.IsTrue(result[2].Item2 == null, "Last token content should be null");
        Assert.IsTrue(result.Count == 3, "Should contain 3 tokens");
    }

    [Test]
    public void parseTest2()
    {
        var def = "test 123";
        var lexer = new Lexer();
        var result = lexer.parse(def);

        Assert.IsTrue(result[0].Item1 == TokenType.Identifier, "First token should be of type identifier");
        Assert.IsTrue(result[0].Item2.Equals("test"), "First token should contain 123");
        Assert.IsTrue(result[1].Item1 == TokenType.Space, "Second token should be of type space");
        Assert.IsTrue(result[1].Item2.Equals(" "), "Second token should contain a space");
        Assert.IsTrue(result[2].Item1 == TokenType.IntLit, "Last token should be of type EDF");
        Assert.IsTrue(result[2].Item2.Equals("123"), "Last token content should be null");
        Assert.IsTrue(result.Count == 4, "Should contain 4 tokens");
    }

    [Test]
    public void parseTest3()
    {
        var def = "test*123+38test";
        var lexer = new Lexer();
        var result = lexer.parse(def);

        Assert.IsTrue(result[0].Item1 == TokenType.Identifier, "First token should be of type identifier");
        Assert.IsTrue(result[0].Item2.Equals("test"), "First token should contain 123");
        Assert.IsTrue(result[1].Item1 == TokenType.Op, "Second token should be of type op");
        Assert.IsTrue(result[1].Item2.Equals("*"), "Second token should contain a *");
        Assert.IsTrue(result[2].Item1 == TokenType.IntLit, "token should be of type EDF");
        Assert.IsTrue(result[2].Item2.Equals("123"), "token content should be null");
        Assert.IsTrue(result[3].Item1 == TokenType.Op, "token should be of type op");
        Assert.IsTrue(result[3].Item2.Equals("+"), "token should contain a +");
        Assert.IsTrue(result.Count == 7, "Should contain 7 tokens");
    }

    [Test]
    public void checkBalancedParTest1()
    {
        var s = "(()())";
        var def = new Definition(s);
        var transform = s.Select(c => (TokenType.Delimiter, c.ToString())).ToList();
        
        Assert.IsTrue(def.checkBalancedPar(transform), "Should be a balanced string");
    }

    [Test]
    public void checkBalancedParTest2()
    {
        var s = "(()()))";
        var def = new Definition(s);
        var transform = s.Select(c => (TokenType.Delimiter, c.ToString())).ToList();
        
        Assert.IsFalse(def.checkBalancedPar(transform), "Should not be a balanced string");
    }

    [Test]
    public void isValidTest1()
    {
        var s = "(()()))";
        var def = new Definition(s);
        
        Assert.IsFalse(def.isValid(), "Should not be a valid definition : )( pattern is not accepted");
    }

    [Test]
    public void rule45Test1() { 
        var s = "(()())";
        var def = new Definition(s);
        var transform = s.Select(c => (TokenType.Delimiter, c.ToString())).ToList();

        Assert.IsFalse(def.rule45(transform), "Should not be a valid definition : )( pattern is not accepted");

    }

    [Test]
    public void isValidTest2()
    {
        var s = "((123*43)+(243     ))";
        var def = new Definition(s);
        Assert.IsTrue(def.isValid(), "Should be a valid definition");
    }

    [Test]
    public void isValidTest3()
    {
        var s = "x + y * 2";
        var def = new Definition(s);
        var env = new List<string>(){"x", "y"};
        Assert.IsTrue(def.isValid(env), "Should be a valid definition");
    }

    [Test]
    public void isValidTest4()
    {
        var s = "x + y * 2";
        var def = new Definition(s);
        var env = new List<string>(){"x"};
        Assert.IsFalse(def.isValid(env), "Should not be a valid definition");
    }

    [Test]
    public void findSubListTest1()
    {
        var lexer = new Lexer();
        var input = "(123+4)";
        var tokens = lexer.parse(input);
        var def = new Definition(input);

        var (res, index) = def.findSubList(tokens);
        Assert.IsTrue(res.Count == 3);
        Assert.IsTrue(res[0].Item1 is TokenType.IntLit);

    }

    [Test]
    public void findSubListTest2()
    {
        var lexer = new Lexer();
        var input = "(b1 | b1) | b1 | b1";
        var tokensS = lexer.parse(input);
        var tokens = tokensS.Where(token => token.Item1 != TokenType.Space).ToList();
        var def = new Definition(input);

        var (res, index) = def.findSubList(tokens);
        printTokens(res);
        Assert.IsTrue(res.Count == 3, "Length");
        Assert.IsTrue(res[0].Item1 is TokenType.Identifier, "Type");

    }

    [Test]
    public void gatherTest1()
    {
        var lexer = new Lexer();
        var input = "b1 | b1";
        var tokensS = lexer.parse(input);
        var tokens = tokensS.Where(token => token.Item1 != TokenType.Space).ToList();
        var def = new Definition(input);

        var b1 = Variable.Bernoulli(0.5);
        var env = new List<(Variable<bool>, string)>(){(b1, "b1")};
        printTokens(tokens);
        var res = def.gather(tokens, env);
    }

    [Test]
    public void gatherTest2()
    {
        var lexer = new Lexer();
        var input = "b1 & (b2 | b3)";
        var tokensS = lexer.parse(input);
        var tokens = tokensS.Where(token => token.Item1 != TokenType.Space).ToList();
        var def = new Definition(input);

        var b1 = Variable.Bernoulli(0.5);
        var b2 = Variable.Bernoulli(0.5);
        var b3 = Variable.Bernoulli(0.5);
        var env = new List<(Variable<bool>, string)>(){(b1, "b1"),(b2, "b2"),(b3, "b3")};
        var res = def.gather(tokens, env);

        var b4 = b1 & (b2 | b3);

        var b4inf = engine.Infer(b4);
        var resinf = engine.Infer(res);
        Debug.Log("Via gather : " + resinf);
        Debug.Log("Via Infer : " + b4inf);

        Assert.IsTrue(b4inf.Equals(resinf)," Should have the same distribution");

    }

    [Test]
    public void gatherTest3()
    {
        var lexer = new Lexer();
        var input = "b1 | b2";
        var tokensS = lexer.parse(input);
        var tokens = tokensS.Where(token => token.Item1 != TokenType.Space).ToList();
        var def = new Definition(input);

        var b1 = Variable.Bernoulli(0.5);
        var b2 = Variable.Bernoulli(0.5);
        var env = new List<(Variable<bool>, string)>(){(b1, "b1"), (b2, "b2")};
        var res = def.gather(tokens, env);

        var b3 = b1 | b2;

        var b2inf = engine.Infer(b3);
        var resinf = engine.Infer(res);

        Assert.IsTrue(b2inf.Equals(resinf)," Should have the same distribution");
    }

    [Test]
    public void gatherTest4()
    {
        var lexer = new Lexer();
        var input = "((b1 | b2))";
        var tokensS = lexer.parse(input);
        var tokens = tokensS.Where(token => token.Item1 != TokenType.Space).ToList();
        var def = new Definition(input);

        var b1 = Variable.Bernoulli(0.5);
        var b2 = Variable.Bernoulli(0.5);
        var env = new List<(Variable<bool>, string)>(){(b1, "b1"), (b2, "b2")};
        var res = def.gather(tokens, env);

        var b3 = ((b1 | b2));

        var b2inf = engine.Infer(b3);
        var resinf = engine.Infer(res);

        Assert.IsTrue(b2inf.Equals(resinf)," Should have the same distribution");
    }

    [Test]
    public void gatherTest5()
    {
        var lexer = new Lexer();
        var input = "((b1 | b2))";
        var tokensS = lexer.parse(input);
        var tokens = tokensS.Where(token => token.Item1 != TokenType.Space).ToList();
        var def = new Definition(input);

        var b1 = Variable.Bernoulli(0.5);
        var b2 = Variable.Bernoulli(0.5);
        var env = new List<(Variable<bool>, string)>(){(b1, "b1"), (b2, "b2")};
        var res = def.gather(tokens, env);

        var b3 = ((b1 | b2));

        var b3inf = engine.Infer(b3);
        var resinf = engine.Infer(res);
        Debug.Log("Via gather : " + resinf);
        Debug.Log("Via Infer : " + b3inf);
        Assert.IsTrue(b3inf.Equals(resinf)," Should have the same distribution");
    }

    [Test]
    public void gatherTest7()
    {
        var lexer = new Lexer();
        var input = "((b1 | (b2)))";
        var tokensS = lexer.parse(input);
        var tokens = tokensS.Where(token => token.Item1 != TokenType.Space).ToList();
        var def = new Definition(input);

        var b1 = Variable.Bernoulli(0.5);
        var b2 = Variable.Bernoulli(0.5);
        var env = new List<(Variable<bool>, string)>(){(b1, "b1"), (b2, "b2")};
        var res = def.gather(tokens, env);

        var b3 = ((b1 | (b2)));

        var b3inf = engine.Infer(b3);
        var resinf = engine.Infer(res);
        Debug.Log("Via gather : " + resinf);
        Debug.Log("Via Infer : " + b3inf);
        Assert.IsTrue(b3inf.Equals(resinf)," Should have the same distribution");
    }

    [Test]
    public void gatherTest8()
    {
        var lexer = new Lexer();
        var input = "(((b1) | b2))";
        var tokensS = lexer.parse(input);
        var tokens = tokensS.Where(token => token.Item1 != TokenType.Space).ToList();
        var def = new Definition(input);

        var b1 = Variable.Bernoulli(0.5);
        var b2 = Variable.Bernoulli(0.5);
        var env = new List<(Variable<bool>, string)>(){(b1, "b1"), (b2, "b2")};
        var res = def.gather(tokens, env);

        var b3 = (((b1) | b2));

        var b3inf = engine.Infer(b3);
        var resinf = engine.Infer(res);
        Debug.Log("Via gather : " + resinf);
        Debug.Log("Via Infer : " + b3inf);
        Assert.IsTrue(b3inf.Equals(resinf)," Should have the same distribution");
    }

    [Test]
    public void gatherTest9()
    {
        var lexer = new Lexer();
        var input = "((b1) & (b2 | b3))";
        var tokensS = lexer.parse(input);
        var tokens = tokensS.Where(token => token.Item1 != TokenType.Space).ToList();
        var def = new Definition(input);

        var b1 = Variable.Bernoulli(0.5);
        var b2 = Variable.Bernoulli(0.5);
        var b3 = Variable.Bernoulli(0.5);
        var env = new List<(Variable<bool>, string)>(){(b1, "b1"),(b2, "b2"),(b3, "b3")};
        var res = def.gather(tokens, env);

        var b4 = ((b1) & (b2 | b3));

        var b4inf = engine.Infer(b4);
        var resinf = engine.Infer(res);
        Debug.Log("Via gather : " + resinf);
        Debug.Log("Via Infer : " + b4inf);

        Assert.IsTrue(b4inf.Equals(resinf)," Should have the same distribution");

    }

    [Test]
    public void gatherTest6()
    {
        var lexer = new Lexer();
        var input = "(((b1)))";
        var tokensS = lexer.parse(input);
        var tokens = tokensS.Where(token => token.Item1 != TokenType.Space).ToList();
        var def = new Definition(input);

        var b1 = Variable.Bernoulli(0.5);
        var env = new List<(Variable<bool>, string)>(){(b1, "b1")};
        var res = def.gather(tokens, env);

        var b3 = (((b1)));

        var b3inf = engine.Infer(b3);
        var resinf = engine.Infer(res);
        Debug.Log("Via gather : " + resinf);
        Debug.Log("Via Infer : " + b3inf);
        Assert.IsTrue(b3inf.Equals(resinf)," Should have the same distribution");
    }

    private void printTokens(List<(TokenType, string)> tokens) {
        foreach(var (type, c) in tokens) { 
            Debug.Log("[" + type + ", " + c + " ]");
        }
    }

}