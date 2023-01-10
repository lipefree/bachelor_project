using System.Collections.Generic;
using System.Linq;
using Microsoft.ML.Probabilistic.Models;
using UnityEngine;

public class Definition
{

    private string definition;
    private Lexer lexer;
    public Definition(string definition)
    {
        this.definition = definition;
        this.lexer = new Lexer();
    }

    /// <summary>
    /// interpret the field 'definition' into an infer.NET variable
    /// </summary>
    /// <param name="env"> List of variables environement.</param>
    /// <returns>An infer.NET variable.</returns>
    public Variable<bool> interpret(List<(Variable<bool>, string)> env)
    {
        var tokensWithSpace = lexer.parse(definition);
        var tokens = tokensWithSpace.Where(token => token.Item1 != TokenType.Space).ToList();

        return gather(tokens, env);
    }

    /// <summary>
    /// Consume the tokens to generate an infer.NET variable
    /// </summary>
    /// <param name="tokens"> List of tokens.</param>
    /// <param name="env"> List of variables environement.</param>
    /// <returns>An infer.NET variable.</returns>
    public Variable<bool> gather(List<(TokenType, string)> tokens, List<(Variable<bool>, string)> env)
    {
        //The eating parameter is for debug purposes
        Variable<bool> gatherAcc(List<(TokenType, string)> tokens, List<(Variable<bool>, string)> env, Variable<bool> acc, string eating)
        {
            if (tokens.Count <= 1)
            {
                return acc;
            }

            if (tokens.First().Item1 is TokenType.EDF)
            {
                return acc;
            }
            var nextOp = tokens.First();
            var nextVar = tokens.Skip(1).First();
            Variable<bool> z;
            if (nextVar.Item1 is TokenType.Delimiter && nextVar.Item2.Equals("("))
            {
                var (innerTokens, index) = findSubList(tokens.Skip(1).ToList());
                eating += eat(tokens.Take(index + 1).ToList());
                tokens = tokens.Skip(index + 1).ToList();
                z = gather(innerTokens, env);
            }
            else
            {
                z = env.Where(v => v.Item2.Equals(tokens.Skip(1).First().Item2)).First().Item1;
                eating += eat(tokens.Take(1).ToList());
                tokens = tokens.Skip(2).ToList();
            }
            return gatherAcc(tokens, env, consume(acc, nextOp, z), eating);
        }

        var firstToken = tokens.First();
        Variable<bool> z;
        string eating;
        if (firstToken.Item1 is TokenType.Delimiter && firstToken.Item2.Equals("("))
        {
            var (innerTokens, index) = findSubList(tokens);
            eating = eat(tokens.Take(index).ToList());
            tokens = tokens.Skip(index).ToList();
            z = gather(innerTokens, env);

        }
        else
        {
            z = env.Where(v => v.Item2.Equals(tokens.First().Item2)).ToList().First().Item1;
            eating = eat(tokens.Take(1).ToList());
            tokens = tokens.Skip(1).ToList();
        }
        return gatherAcc(tokens, env, z, eating);
    }

    /// <summary>
    /// Consume one token to generate an infer.NET variable
    /// </summary>
    /// <param name="z"> Variable accumulator.</param>
    /// <param name="t1"> Operation token.</param>
    /// <param name="next"> Variable to process.</param>
    /// <returns>An infer.NET variable.</returns>
    public Variable<bool> consume(Variable<bool> z, (TokenType, string) t1, Variable<bool> next)
    {
        var (type1, s1) = t1;

        if (!(type1 is TokenType.Op))
        {
            return null;
        }

        switch (s1)
        {
            case "|":
                return z | next;
            case "&":
                return z & next;
            default:
                return null;
        }

    }
    
    /// <summary>
    /// Check if the tokens have balanced parenthesis
    /// </summary>
    /// <param name="tokens"> List of tokens.</param>
    /// <returns>whether the parenthesis are balanced or not.</returns>
    public bool checkBalancedPar(List<(TokenType, string)> tokens)
    {
        int balanced = 0;

        foreach (var (type, c) in tokens)
        {
            if (type == TokenType.Delimiter)
            {
                if (c.Equals("("))
                {
                    balanced += 1;
                }
                else if (c.Equals(")"))
                {
                    balanced -= 1;
                }

                if (balanced < 0)
                {
                    return false;
                }
            }
        }

        return balanced == 0 ? true : false;
    }

    /// <summary>
    /// Find the closing parenthesis and returns the sublist of tokens inside the range
    /// </summary>
    /// <param name="tokens"> List of tokens.</param>
    /// <returns>The sublist of tokens and the index of the closing parenthesis.</returns>
    public (List<(TokenType, string)>, int) findSubList(List<(TokenType, string)> tokens)
    {
        int balanced = 0;
        int index = 0;
        foreach (var (type, c) in tokens)
        {
            if (c.Equals("("))
            {
                balanced += 1;
            }
            else if (c.Equals(")"))
            {
                balanced -= 1;
            }

            if (balanced == 0)
            {
                break;
            }
            index++;
        }

        return (tokens.Skip(1).Take(index - 1).ToList(), index + 1);
    }

    private string eat(List<(TokenType, string)> tokens)
    {
        if (tokens.Count == 0)
        {
            return "";
        }

        return tokens.First().Item2 + eat(tokens.Skip(1).ToList());
    }

    private void printTokens(List<(TokenType, string)> tokens)
    {
        foreach (var (type, c) in tokens)
        {
            Debug.Log("[" + type + ", " + c + " ]");
        }
    }
}




public class Lexer
{
    public Lexer()
    {

    }

    public List<(TokenType, string)> parse(string definition)
    {
        List<(TokenType, string)> parseAcc(string definition, List<(TokenType, string)> acc)
        {
            if (definition.Length == 0)
            {
                acc.Add((TokenType.EDF, null));
                return acc;
            }

            var (type, word) = lmp(definition);
            acc.Add((type, word));
            return parseAcc(definition.Substring(word.Length), acc);
        }

        return parseAcc(definition, new List<(TokenType, string)>());
    }

    public (TokenType, string) lmp(string definition)
    {
        if (definition.Length == 0)
        {
            return (TokenType.EDF, definition);
        }

        var initialTypeCheck = kindOf(definition[0]);
        TokenType initialType;
        List<TokenType> acceptedType;
        if (initialTypeCheck.Contains(TokenType.Identifier) && initialTypeCheck.Contains(TokenType.IntLit))
        {
            initialType = TokenType.IntLit;
            acceptedType = initialTypeCheck;
        }
        else
        {
            initialType = initialTypeCheck[0];
            acceptedType = new List<TokenType>() { initialType };
        }

        if (initialType is TokenType.Op || initialType is TokenType.Delimiter)
        {
            return (initialType, definition.First().ToString());
        }

        int i = 1;
        while (i < definition.Length)
        {
            var nextType = kindOf(definition[i]);
            if (!compareTypes(nextType, acceptedType, initialType))
            {
                return (initialType, definition.Substring(0, i));
            }
            i++;
        }
        return (initialType, definition);
    }

    bool compareTypes(List<TokenType> newType, List<TokenType> acceptedType, TokenType initialType)
    {

        if (initialType == TokenType.IntLit)
        { //TODO: Redo the logic sometimes, this is a special case to handle string like "123test" (run test).
            if (newType.Contains(TokenType.Identifier) && newType.Count == 1)
            {
                return false;
            }
        }

        foreach (var type1 in newType)
        {
            if (acceptedType.Contains(type1))
            {
                return true;
            }
        }

        return false;
    }

    public List<TokenType> kindOf(char c)
    {
        var identifier = new List<string>(){
            "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z",
            "0","1","2","3","4","5","6","7","8","9"
        };

        var intLit = new List<string>(){
            "0","1","2","3","4","5","6","7","8","9"
        };

        var delitimiter = new List<string>(){
            "(", ")"
        };

        var op = new List<string>{
            "+", "-", "*", "&", "|" // TODO: / is not supported as it is error prone
        };

        var space = new List<string>{
            " "
        };

        var types = new List<List<string>>() { identifier, intLit, delitimiter, op, space };

        var actualTypes = types.Where(type => type.Contains(c.ToString()));

        var tokenTypes = new List<TokenType>();

        if (actualTypes.Contains(identifier))
        {
            tokenTypes.Add(TokenType.Identifier);
        }

        if (actualTypes.Contains(intLit))
        {
            tokenTypes.Add(TokenType.IntLit);
        }

        if (actualTypes.Contains(delitimiter))
        {
            tokenTypes.Add(TokenType.Delimiter);
        }

        if (actualTypes.Contains(op))
        {
            tokenTypes.Add(TokenType.Op);
        }

        if (actualTypes.Contains(space))
        {
            tokenTypes.Add(TokenType.Space);
        }

        if (actualTypes.ToList().Count == 0)
        {
            tokenTypes.Add(TokenType.NotDefined);
        }

        return tokenTypes;
    }
}

public enum TokenType
{
    Identifier,  //variable name "x" or "variable1213", note : Identifier can't begin by an intLit.
    IntLit, //integer literal "12"
    Delimiter, // ( or )
    Op, // +, -, *, &, |, note : the word "operator" is reserved in C#.
    Space,
    NotDefined,
    EDF, // End Of Definition
}

