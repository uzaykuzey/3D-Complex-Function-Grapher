parser grammar MathParser;

options { tokenVocab=MathLexer; }


add_expr: add_expr add_op mult_expr | add_op add_expr | mult_expr ;
mult_expr: mult_expr mult_op exp_expr | mult_expr exp_expr | exp_expr;
exp_expr: final_expr exp_op exp_expr | final_expr;
final_expr: constant 
           | variable 
           | function final_expr
           | POW LP add_expr COMMA add_expr RP
           | BETA LP add_expr COMMA add_expr RP
           | LOG LP add_expr COMMA add_expr RP
           | LOG final_expr
           | sum_or_product LP add_expr COMMA ITERATIVE_VARIABLE COMMA int_declaration_for_iteration COMMA int_declaration_for_iteration RP
           | ABSSYMBOL add_expr ABSSYMBOL
           | LP add_expr RP;
           
exp_op: EXPSYMBOL;
add_op: PLUS | MINUS;
mult_op: TIMES | DIVIDE | MOD;

int_declaration_for_iteration: add_op INTEGER | INTEGER;

function: EXP | SQRT | CBRT | SIN | COS | TAN | COT | SEC | CSC 
         | ASIN | ACOS | ATAN | ACOT | LN | ABS | RE | IM 
         | GAMMA | ZETA | W | ERF | ARG | SI | SIGN;
constant: INTEGER | RATIONAL | I | E | PI | PHI | APERY | EULERMASCHERONI;
variable: X | Y | Z | ITERATIVE_VARIABLE;
sum_or_product: SUM | PRODUCT;