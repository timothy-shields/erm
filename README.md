[Graphviz]: (http://www.graphviz.org/)

**erm** is a tiny language for concisely specifying entity-relationship models.

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

    ermc render -i=education.erm
    
The rendered SVG follows.

![education.svg](http://i.imgur.com/4bDvDse.png)

> [Graphviz][] must be installed for `ermc` to perform the rendering.
> Also, in `ermc.exe.config`, the `graphvizBin` value must be set
> to the `bin` directory of the [Graphviz][] installation.

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
