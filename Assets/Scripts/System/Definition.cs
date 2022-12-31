using System.Collections.Generic;
using System.Linq;
using Microsoft.ML.Probabilistic.Models;
using UnityEngine;

public class Definition {

    private string definition;
    private Lexer lexer;
    public Definition(string definition)
    {
        this.definition = definition;
        this.lexer = new Lexer();
    }

    public bool isValid(List<string> env = null)
    {   
        //TODO: We should use a proper grammar, but it will take too much trials and errors 

        //NOTE : This function is deprecated, we will just use the built in enginer.infer(interpret(definition))
        // and catch if there is an error 
        var tokensWithSpace = lexer.parse(definition);
        var tokens = tokensWithSpace.Where(token => token.Item1 != TokenType.Space).ToList();
        // printTokens(tokens);
        //1. Every definition should contain an End Of Definition Token at the end.
        if(tokens.Last().Item1 != TokenType.EDF) {
            Debug.Log("Rule 1");
            return false;
        }

        //2a. We should not find notDefined tokens in it
        //2b. EDF tokens should only be found at the end
        if(!rule2(tokens)) {
            Debug.Log("Rule 2");
            return false;
        }

        //3. We should have balanced parenthesis
        if(!checkBalancedPar(tokens)){
            Debug.Log("Rule 3");
            return false;
        }

        //4. If indentifier or an intLit then the next token can be : op or EDF or closing brackets
        //5. If closing bracket then the next token can be : brackets or op or EDF
        if(!rule45(tokens)) { 
            return false;
        }

        //6. All identifier should be defined in the environment
        foreach(var (type, identifier) in tokens.Where(token => token.Item1 is TokenType.Identifier)){
            if(!env.Contains(identifier)) {
                Debug.Log("Rule 6");
                return false;
            }
        }

        //TODO: Some rules are not yet implemented
        //7. op should be between 2 expressions : () + 2 is not valid but ((4)) + 3 is valid
            
        return true;
    }

    //TODO: we should be able to handle all kind of variable not just bools.
    public Variable<bool> interpret(List<(Variable<bool>, string)> env) {
        var tokensWithSpace = lexer.parse(definition);
        var tokens = tokensWithSpace.Where(token => token.Item1 != TokenType.Space).ToList();

        return gather(tokens, env);
    }

    public Variable<bool> gather(List<(TokenType, string)> tokens, List<(Variable<bool>, string)> env) {

        Variable<bool> gatherAcc(List<(TokenType, string)> tokens, List<(Variable<bool>, string)> env, Variable<bool> acc, string eating) { 
            // Debug.Log("-----------------------------------");
            // printTokens(tokens);
            if(tokens.Count <= 1) {
                return acc;
            }

            if(tokens.First().Item1 is TokenType.EDF) {
                return acc;
            }
            var nextOp = tokens.First();
            var nextVar = tokens.Skip(1).First();
            // Debug.Log("Op : " + nextOp.Item2);
            // Debug.Log("Var : " + nextVar.Item2);
            Variable<bool> z;
            if(nextVar.Item1 is TokenType.Delimiter && nextVar.Item2.Equals("(")) {
                var (innerTokens, index) = findSubList(tokens.Skip(1).ToList());
                eating += eat(tokens.Take(index + 1).ToList());
                tokens = tokens.Skip(index + 1).ToList();
                // Debug.Log("ENTER");
                // printTokens(tokens);
                z = gather(innerTokens, env);
            } else { 
                z = env.Where(v => v.Item2.Equals(tokens.Skip(1).First().Item2)).First().Item1;
                eating += eat(tokens.Take(1).ToList());
                tokens = tokens.Skip(2).ToList();
            }
            // Debug.Log("EAT : " + eating);
            return gatherAcc(tokens, env, consume(acc, nextOp, z), eating);
        }

        // Debug.Log("nb of tokens : " + tokens.Count);
        // printTokens(tokens);
        var firstToken = tokens.First();
        Variable<bool> z;

        string eating;
        if(firstToken.Item1 is TokenType.Delimiter && firstToken.Item2.Equals("(")) { 
            var (innerTokens, index) = findSubList(tokens);
            eating = eat(tokens.Take(index).ToList());
            tokens = tokens.Skip(index).ToList();
            z = gather(innerTokens, env);
            
        } else {
            z = env.Where(v => v.Item2.Equals(tokens.First().Item2)).ToList().First().Item1;
            eating = eat(tokens.Take(1).ToList());
            tokens = tokens.Skip(1).ToList();
        }
        // Debug.Log("EAT : " + eating);
        return gatherAcc(tokens, env, z, eating);
    }

    public Variable<bool> consume(Variable<bool> z, (TokenType, string) t1, Variable<bool> next) {
        var (type1, s1) = t1;

        if(!(type1 is TokenType.Op)) {
            return null;
        }

        switch(s1) { 
            case "|":
                return z | next;
            case "&":
                return z & next;
            default:
                return null;
        }

    }

    public bool checkBalancedPar(List<(TokenType, string)> tokens) {
        int balanced = 0;

        foreach(var (type, c) in tokens){
            if(type == TokenType.Delimiter) {
                if(c.Equals("(")) { 
                    balanced += 1;
                } else if(c.Equals(")")) {
                    balanced -= 1;
                }

                if(balanced < 0) {
                    return false;
                }
            }
        }

        return balanced==0?true:false;
    }

    public (List<(TokenType, string)>, int) findSubList(List<(TokenType, string)> tokens) {
        //Find closing bracket
        int balanced = 0;
        int index = 0; 
        foreach(var (type, c) in tokens) { 
            if(c.Equals("(")) { 
                balanced += 1;
            } else if(c.Equals(")")) {
                balanced -= 1;
            }

            if(balanced == 0) { 
                break;
            }
            index++;
        }

        return (tokens.Skip(1).Take(index - 1).ToList(), index + 1);
    }

    private string eat(List<(TokenType, string)> tokens) { 
        if(tokens.Count == 0) {
            return "";
        }

        return tokens.First().Item2 + eat(tokens.Skip(1).ToList());
    }

    public bool rule2(List<(TokenType, string)> tokens) {
        if(tokens.Count == 1) { 
            return tokens.First().Item1 == TokenType.EDF?true:false;
        }

        if(tokens.First().Item1 == TokenType.NotDefined) {
            return false;
        }

        if(tokens.Count > 1 && tokens.First().Item1 == TokenType.EDF) {
            return false;
        }

        return rule2(tokens.Skip(1).ToList());
    }

    public bool rule45(List<(TokenType, string)> tokens) { 
        //4. If indentifier or an intLit then the next token can be : op or EDF or closing brackets
        //5. If closing bracket then the next token can be : brackets or op or EDF 

        if(tokens.Count <= 1) {
            return true;
        }

        var (type, c) = tokens.First();
        var (nextType, c2) = tokens.Skip(1).First();

        //rule 4
        if(type is TokenType.Identifier || type is TokenType.IntLit) {
            if(nextType is TokenType.Op || nextType is TokenType.EDF) {
                return rule45(tokens.Skip(1).ToList());
            } else if(nextType is TokenType.Delimiter && c2.Equals(")")){
                return rule45(tokens.Skip(1).ToList());
            } else { 
                Debug.Log("Rule 4");
                return false;
            }
        }

        //rule 5
        if(type is TokenType.Delimiter && c.Equals(")")) {
            if(nextType is TokenType.EDF || nextType is TokenType.Op){
                return rule45(tokens.Skip(1).ToList());
            } else if(nextType is TokenType.Delimiter && c2.Equals(")")) {
                return rule45(tokens.Skip(1).ToList());
            } else { 
                Debug.Log("Rule 5");
                return false;
            }
        }

        return rule45(tokens.Skip(1).ToList());
    }

    private void printTokens(List<(TokenType, string)> tokens) {
        foreach(var (type, c) in tokens) { 
            Debug.Log("[" + type + ", " + c + " ]");
        }
    }
}




public class Lexer { 
    public Lexer()
    {

    }

    public List<(TokenType, string)> parse(string definition)
    {
        List<(TokenType, string)> parseAcc(string definition, List<(TokenType, string)> acc) { 
            if(definition.Length == 0) { 
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
        if(definition.Length == 0) { 
            return (TokenType.EDF, definition);
        }

        var initialTypeCheck = kindOf(definition[0]);
        TokenType initialType;
        List<TokenType> acceptedType;
        if(initialTypeCheck.Contains(TokenType.Identifier) && initialTypeCheck.Contains(TokenType.IntLit)) { 
            initialType = TokenType.IntLit;
            acceptedType = initialTypeCheck;
        } else { 
            initialType = initialTypeCheck[0];
            acceptedType = new List<TokenType>(){initialType};
        }

        if(initialType is TokenType.Op || initialType is TokenType.Delimiter){
            return (initialType, definition.First().ToString());
        }

        int i = 1;
        while(i < definition.Length) 
        {
            var nextType = kindOf(definition[i]);
            if(!compareTypes(nextType, acceptedType, initialType)) { 
                return (initialType, definition.Substring(0, i));
            }
            i++;
        }
        return (initialType, definition);
    }

    bool compareTypes(List<TokenType> newType, List<TokenType> acceptedType, TokenType initialType){

        if(initialType == TokenType.IntLit) { //TODO: Redo the logic sometimes, this is a special case to handle string like "123test" (run test).
            if(newType.Contains(TokenType.Identifier) && newType.Count == 1){
                return false;
            }
        }

        foreach(var type1 in newType){
            if(acceptedType.Contains(type1)) { 
                return true;
            }
        }

        return false;
    }

    public List<TokenType> kindOf(char c) {
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

        var types = new List<List<string>>(){identifier, intLit, delitimiter,op, space};

        var actualTypes = types.Where(type => type.Contains(c.ToString()));

        var tokenTypes = new List<TokenType>();

        if(actualTypes.Contains(identifier)) {
            tokenTypes.Add(TokenType.Identifier);
        }

        if(actualTypes.Contains(intLit)) {
            tokenTypes.Add(TokenType.IntLit);
        }

        if(actualTypes.Contains(delitimiter)) {
            tokenTypes.Add(TokenType.Delimiter);
        }

        if(actualTypes.Contains(op)) {
            tokenTypes.Add(TokenType.Op);
        }

        if(actualTypes.Contains(space)) {
            tokenTypes.Add(TokenType.Space);
        }

        if(actualTypes.ToList().Count == 0){
            tokenTypes.Add(TokenType.NotDefined);
        }

        return tokenTypes;
    }
}

public enum TokenType { 
    Identifier,  //variable name "x" or "variable1213", note : Identifier can't begin by an intLit.
    IntLit, //integer literal "12"
    Delimiter, // ( or )
    Op, // +, -, *, &, |, note : the word "operator" is reserved in C#.
    Space, 
    NotDefined,
    EDF, // End Of Definition
}

