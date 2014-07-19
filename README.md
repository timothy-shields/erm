# erm

A tiny language for specifying entity-relationship models.

## Example

Define an entity-relationship model file like the following.

    [Person|1] <Attends> [School|*];
    [Person] -> [Student] [Teacher];
    [School|*] <Offers> [Major|*];
    [Student|1] <Studies> [Major|*];
    [Student|+] <Takes> [Class|*];
    [Teacher|*] <Teaches> [Class|1];
    [Class|?] <Requires> [Major|*];
    
Let's call it `education.erm`. Using the `ermc` compiler, compile and render your model.
(Requires [Graphviz](http://www.graphviz.org/) is installed.)

    ermc render -i=education.erm
    
The rendered SVG follows.

![example](http://i.imgur.com/4bDvDse.png)

## Language Specification

The `erm` language is specified by the following grammar,
where `Decls` is the root nonterminal.
(Whitespace is not required between tokens, but it is allowed.)

    Id ::= [a-zA-Z]+
    Cardinality ::= '?' | '1' | '*' | '+'
    EntityExpr ::= '[' Id ']'
    RelatedEntityExpr ::= '[' Id '|' Cardinality ']'
    RelationshipExpr ::= '<' Id '>'
    RelationshipDecl ::= RelatedEntityExpr+ RelationshipExpr RelatedEntityExpr+
    UnionDecl ::= EntityExpr '->' EntityExpr+
    Decl ::= RelationshipDecl | UnionDecl
    Decls ::= Decl*
