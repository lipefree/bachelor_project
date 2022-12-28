using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Definition {

    private string definition;
    public Definition(string definition)
    {
        this.definition = definition;
    }

    public bool isValid()
    {

        return false;
    }
}




public class Lexer { 
    public Lexer()
    {

    }

    public List<(TokenType, string)> parse(string definition)
    {
        // I guess on va longuest match

        return null;
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

        int i = 1;
        while(i < definition.Length) 
        {
            var nextType = kindOf(definition[i]);
            if(!compareTypes(nextType, acceptedType, initialType)) { 
                Debug.Log("early break");
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
            "(", ")" //TODO: Add [] and {} later maybe
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
    Identifier,  //variable name "x" or "variable1213" 
    IntLit, //integer literal "12"
    Delimiter, // ( or )
    Op, // +, -, *, &, |, note : the word "operator" is reserved in C#.
    Space, 
    NotDefined,
    EDF, // End Of Definition
}


public class CheckKindOfTest1{
    public void test(){

    }
}
    
