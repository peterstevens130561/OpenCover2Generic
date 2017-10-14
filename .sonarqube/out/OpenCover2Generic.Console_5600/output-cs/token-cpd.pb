¹
lE:\users\stevpet\my documents\visual studio 2015\Projects\OpenCover2Generic\OpenCover2Generic\BranchPoint.cs
	namespace 	
BHGE
 
. 
	SonarQube 
. 
OpenCover2Generic *
{ 
internal 
class 
BranchPoint 
:  
IBranchPoint! -
{		 
private

 
readonly

 
int

 
paths

 "
;

" #
private 
readonly 
int 
visitedCount )
;) *
public 
BranchPoint 
( 
Boolean "
	isVisited# ,
), -
:. /
this0 4
(4 5
	isVisited5 >
?? @
$numA B
:C D
$numE F
,F G
$numH I
)I J
{ 	
}
 
private 
BranchPoint 
( 
int  
branchesVisited! 0
,0 1
int2 5
pathId6 <
)< =
{ 	
this 
. 
visitedCount 
= 
branchesVisited  /
;/ 0
this 
. 
paths 
= 
pathId 
;  
} 	
public 
int 
Paths 
{ 	
get 
{   
return!! 
paths!! 
;!! 
}"" 
}## 	
public%% 
int%% 
PathsVisited%% 
{&& 	
get'' 
{(( 
return)) 
visitedCount)) #
;))# $
}** 
}++ 	
public-- 
IBranchPoint-- 
Add-- 
(--  
IBranchPoint--  ,
branchPoint--- 8
)--8 9
{.. 	
return// 
new// 
BranchPoint// "
(//" #
visitedCount//# /
+//0 1
branchPoint//2 =
.//= >
PathsVisited//> J
,//J K
branchPoint//L W
.//W X
Paths//X ]
+//] ^
Paths//^ c
)//c d
;//d e
}00 	
}11 
}22 Á
rE:\users\stevpet\my documents\visual studio 2015\Projects\OpenCover2Generic\OpenCover2Generic\CommandLineParser.cs
	namespace 	
BHGE
 
. 
	SonarQube 
. 
OpenCover2Generic *
{ 
class		 	
CommandLineParser		
 
:		 
ICommandLineParser		 0
{

 
public 
string 
[ 
] 
Args 
{ 
get "
;" #
set$ '
;' (
}) *
public 
string 
GenericPath !
(! "
)" #
{ 	
return 
GetArgument 
( 
$str *
)* +
;+ ,
} 	
public 
string 
OpenCoverPath #
(# $
)$ %
{ 	
return 
GetArgument 
( 
$str ,
), -
;- .
} 	
private 
string 
GetArgument "
(" #
string# )
key* -
)- .
{ 	
foreach 
( 
string 
arg 
in !
Args" &
)& '
{( )
if 
( 
arg 
. 

StartsWith !
(! "
key" %
)% &
)& '
{ 
return 
( 
arg 
.  
	Substring  )
() *
key* -
.- .
Length. 4
)4 5
)5 6
;6 7
} 
} 
throw   
new   
ArgumentException   '
(  ' (
$"  ( *!
commandline argument   * ?
{  ? @
key  @ C
}  C D
 missing  D L
"  L M
)  M N
;  N O
}!! 	
}"" 
}## ©N
jE:\users\stevpet\my documents\visual studio 2015\Projects\OpenCover2Generic\OpenCover2Generic\Converter.cs
	namespace		 	
BHGE		
 
.		 
	SonarQube		 
.		 
OpenCover2Generic		 *
{

 
internal 
class 
	Converter 
: 

IConverter )
{ 
private 
readonly 
IModel 
model  %
;% &
public 
	Converter 
( 
IModel 
model  %
)% &
{ 	
this 
. 
model 
= 
model 
; 
} 	
public 
void 
Convert 
( 
StreamWriter (
writer) /
,/ 0
StreamReader1 =
reader> D
)D E
{ 	
using 
( 
XmlTextWriter  
	xmlWriter! *
=+ ,
new- 0
XmlTextWriter1 >
(> ?
writer? E
)E F
)F G
{ 
	xmlWriter 
. 

Formatting $
=% &

Formatting' 1
.1 2
Indented2 :
;: ;
	xmlWriter 
. 
Indentation %
=& '
$num( )
;) *
	xmlWriter 
. 
WriteStartDocument ,
(, -
)- .
;. /
	xmlWriter 
. 
WriteStartElement +
(+ ,
$str, 6
)6 7
;7 8
	xmlWriter 
.  
WriteAttributeString .
(. /
$str/ 8
,8 9
$str: =
)= >
;> ?
using 
( 
	XmlReader  
	xmlReader! *
=+ ,
	XmlReader- 6
.6 7
Create7 =
(= >
reader> D
)D E
)E F
{ 
	xmlReader 
. 
MoveToContent +
(+ ,
), -
;- .
while 
( 
	xmlReader $
.$ %
Read% )
() *
)* +
)+ ,
{   
if!! 
(!! 
	xmlReader!! %
.!!% &
NodeType!!& .
==!!/ 1
XmlNodeType!!2 =
.!!= >
Element!!> E
)!!E F
{"" 
switch## "
(##" #
	xmlReader### ,
.##, -
Name##- 1
)##1 2
{$$ 
case%%  $
$str%%% +
:%%+ ,
AddFile&&$ +
(&&+ ,
	xmlReader&&, 5
)&&5 6
;&&6 7
break''$ )
;'') *
case((  $
$str((% 4
:((4 5
AddSequencePoint))$ 4
())4 5
	xmlReader))5 >
)))> ?
;))? @
break**$ )
;**) *
case++  $
$str++% 2
:++2 3
AddBranchPoint,,$ 2
(,,2 3
	xmlReader,,3 <
),,< =
;,,= >
break--$ )
;--) *
case..  $
$str..% -
:..- .
GenerateCoverage//$ 4
(//4 5
	xmlWriter//5 >
,//> ?
model//@ E
)//E F
;//F G
model00$ )
.00) *
Init00* .
(00. /
)00/ 0
;000 1
break11$ )
;11) *
}33 
}44 
}55 
}66 
GenerateCoverage77  
(77  !
	xmlWriter77! *
,77* +
model77, 1
)771 2
;772 3
	xmlWriter88 
.88 
WriteEndElement88 )
(88) *
)88* +
;88+ ,
	xmlWriter:: 
.:: 
WriteEndDocument:: *
(::* +
)::+ ,
;::, -
	xmlWriter;; 
.;; 
Flush;; 
(;;  
);;  !
;;;! "
}<< 
}== 	
private?? 
void?? 
AddFile?? 
(?? 
	XmlReader?? &
	xmlReader??' 0
)??0 1
{@@ 	
stringAA 
fileIdAA 
=AA 
	xmlReaderAA %
.AA% &
GetAttributeAA& 2
(AA2 3
$strAA3 8
)AA8 9
;AA9 :
stringBB 
filePathBB 
=BB 
	xmlReaderBB '
.BB' (
GetAttributeBB( 4
(BB4 5
$strBB5 ?
)BB? @
;BB@ A
modelCC 
.CC 
AddFileCC 
(CC 
fileIdCC  
,CC  !
filePathCC" *
)CC* +
;CC+ ,
}DD 	
privateFF 
voidFF 
AddBranchPointFF #
(FF# $
	XmlReaderFF$ -
	xmlReaderFF. 7
)FF7 8
{GG 	
stringHH 
fileIdHH 
;HH 
stringII 

sourceLineII 
=II 
	xmlReaderII  )
.II) *
GetAttributeII* 6
(II6 7
$strII7 ;
)II; <
;II< =
stringJJ 
visitedCountJJ 
=JJ  !
	xmlReaderJJ" +
.JJ+ ,
GetAttributeJJ, 8
(JJ8 9
$strJJ9 =
)JJ= >
;JJ> ?
fileIdKK 
=KK 
	xmlReaderKK 
.KK 
GetAttributeKK +
(KK+ ,
$strKK, 4
)KK4 5
;KK5 6
modelLL 
.LL 
AddBranchPointLL  
(LL  !
fileIdLL! '
,LL' (

sourceLineLL) 3
,LL3 4
visitedCountLL5 A
)LLA B
;LLB C
}MM 	
privateOO 
voidOO 
AddSequencePointOO %
(OO% &
	XmlReaderOO& /
	xmlReaderOO0 9
)OO9 :
{PP 	
stringQQ 

sourceLineQQ 
=QQ 
	xmlReaderQQ  )
.QQ) *
GetAttributeQQ* 6
(QQ6 7
$strQQ7 ;
)QQ; <
;QQ< =
stringRR 
visitedCountRR 
=RR  !
	xmlReaderRR" +
.RR+ ,
GetAttributeRR, 8
(RR8 9
$strRR9 =
)RR= >
;RR> ?
stringSS 
fileIdSS 
=SS 
	xmlReaderSS %
.SS% &
GetAttributeSS& 2
(SS2 3
$strSS3 ;
)SS; <
;SS< =
modelTT 
.TT 
AddSequencePointTT "
(TT" #
fileIdTT# )
,TT) *

sourceLineTT+ 5
,TT5 6
visitedCountTT7 C
)TTC D
;TTD E
}UU 	
privateWW 
voidWW 
GenerateCoverageWW %
(WW% &
	XmlWriterWW& /
	xmlWriterWW0 9
,WW9 :
IModelWW; A
modelWWB G
)WWG H
{XX 	
foreachYY 
(YY 
IFileCoverageModelYY &
fileCoverageYY' 3
inYY4 6
modelYY7 <
.YY< =
GetCoverageYY= H
(YYH I
)YYI J
)YYJ K
{ZZ 
	xmlWriter[[ 
.[[ 
WriteStartElement[[ +
([[+ ,
$str[[, 2
)[[2 3
;[[3 4
	xmlWriter\\ 
.\\  
WriteAttributeString\\ .
(\\. /
$str\\/ 5
,\\5 6
fileCoverage\\7 C
.\\C D
FullPath\\D L
)\\L M
;\\M N"
GenerateSequencePoints]] &
(]]& '
	xmlWriter]]' 0
,]]0 1
fileCoverage]]2 >
)]]> ?
;]]? @
}^^ 
}__ 	
privateaa 
staticaa 
voidaa "
GenerateSequencePointsaa 2
(aa2 3
	XmlWriteraa3 <
	xmlWriteraa= F
,aaF G
IFileCoverageModelaaH Z
fileCoverageaa[ g
)aag h
{bb 	
foreachcc 
(cc 
ICoveragePointcc #
sequencePointcc$ 1
incc2 4
fileCoveragecc5 A
.ccA B
SequencePointsccB P
)ccP Q
{dd 
	xmlWriteree 
.ee 
WriteStartElementee +
(ee+ ,
$stree, 9
)ee9 :
;ee: ;
stringff 

sourceLineff !
=ff" #
sequencePointff$ 1
.ff1 2

SourceLineff2 <
.ff< =
ToStringff= E
(ffE F
)ffF G
;ffG H
	xmlWritergg 
.gg  
WriteAttributeStringgg .
(gg. /
$strgg/ ;
,gg; <

sourceLinegg= G
)ggG H
;ggH I
	xmlWriterhh 
.hh  
WriteAttributeStringhh .
(hh. /
$strhh/ 8
,hh8 9
sequencePointhh: G
.hhG H
CoveredhhH O
?hhP Q
$strhhR X
:hhY Z
$strhh[ b
)hhb c
;hhc d
IBranchPointii 
branchPointii (
=ii) *
fileCoverageii+ 7
.ii7 8
BranchPointii8 C
(iiC D

sourceLineiiD N
)iiN O
;iiO P
ifjj 
(jj 
branchPointjj 
!=jj  "
nulljj# '
)jj' (
{kk 
	xmlWriterll 
.ll  
WriteAttributeStringll 2
(ll2 3
$strll3 D
,llD E
branchPointllF Q
.llQ R
PathsllR W
.llW X
ToStringllX `
(ll` a
)lla b
)llb c
;llc d
	xmlWritermm 
.mm  
WriteAttributeStringmm 2
(mm2 3
$strmm3 D
,mmD E
branchPointmmF Q
.mmQ R
PathsVisitedmmR ^
.mm^ _
ToStringmm_ g
(mmg h
)mmh i
)mmi j
;mmj k
}nn 
	xmlWriteroo 
.oo 
WriteEndElementoo )
(oo) *
)oo* +
;oo+ ,
}pp 
	xmlWriterqq 
.qq 
WriteEndElementqq %
(qq% &
)qq& '
;qq' (
}rr 	
}ss 
}tt ã
nE:\users\stevpet\my documents\visual studio 2015\Projects\OpenCover2Generic\OpenCover2Generic\CoveragePoint.cs
	namespace 	
BHGE
 
. 
	SonarQube 
. 
OpenCover2Generic *
{ 
internal 
class 
CoveragePoint  
:! "
ICoveragePoint# 1
{ 
private 
readonly 
int 

sourceLine '
;' (
private 
bool 
covered 
; 
public

 
CoveragePoint

 
(

 
string

 #

sourceLine

$ .
,

. /
bool

0 4
visited

5 <
)

< =
{ 	
this 
. 

sourceLine 
= 
int !
.! "
Parse" '
(' (

sourceLine( 2
)2 3
;3 4
this 
. 
covered 
= 
visited "
;# $
} 	
public 
int 

SourceLine 
{ 
get  #
{$ %
return& ,

sourceLine- 7
;7 8
}9 :
}; <
public 
bool 
Covered 
{ 
get !
{" #
return$ *
covered+ 2
;2 3
}4 5
}6 7
public 
void 
Add 
( 
bool 
visited $
)$ %
{ 	
this 
. 
covered 
= 
covered "
||# %
visited& -
;- .
} 	
} 
} ‚*
rE:\users\stevpet\my documents\visual studio 2015\Projects\OpenCover2Generic\OpenCover2Generic\FileCoverageModel.cs
	namespace 	
BHGE
 
. 
	SonarQube 
. 
OpenCover2Generic *
{ 
internal 
class 
FileCoverageModel $
:% &
IFileCoverageModel' 9
{ 
private		 
readonly		 
string		 
filePath		  (
;		( )
private

 
readonly

 

Dictionary

 #
<

# $
string

$ *
,

* +
ICoveragePoint

+ 9
>

9 :
coveragePoints

; I
=

J K
new

L O

Dictionary

P Z
<

Z [
string

[ a
,

a b
ICoveragePoint

b p
>

p q
(

q r
)

r s
;

s t
private 
readonly 

Dictionary #
<# $
string$ *
,* +
IBranchPoint, 8
>8 9
branchPoints: F
=G H
newI L

DictionaryM W
<W X
stringX ^
,^ _
IBranchPoint` l
>l m
(m n
)n o
;o p
public 
FileCoverageModel  
(  !
string! '
filePath( 0
)0 1
{ 	
this 
. 
filePath 
= 
filePath $
;$ %
} 	
public 
string 
FullPath 
{ 	
get 
{ 
return 
filePath 
;  
} 
} 	
public 
IList 
< 
ICoveragePoint #
># $
SequencePoints% 3
{ 	
get 
{ 
List 
< 
ICoveragePoint #
># $
points% +
=, -
coveragePoints. <
.< =
Values= C
.C D
ToListD J
(J K
)K L
;L M
points 
. 
Sort 
( 
( 
pair1 "
," #
pair2$ )
)) *
=>+ -
pair1. 3
.3 4

SourceLine4 >
.> ?
	CompareTo? H
(H I
pair2I N
.N O

SourceLineO Y
)Y Z
)Z [
;[ \
return 
points 
; 
}   
}!! 	
public## 
void## 
AddSequencePoint## $
(##$ %
string##% +

sourceLine##, 6
,##6 7
string##8 >
visitedCount##? K
)##K L
{$$ 	
Boolean%% 
visited%% 
=%% 
int%% !
.%%! "
Parse%%" '
(%%' (
visitedCount%%( 4
)%%4 5
>%%6 7
$num%%8 9
;%%9 :
if&& 
(&& 
coveragePoints&& 
.&& 
ContainsKey&& *
(&&* +

sourceLine&&+ 5
)&&5 6
)&&6 7
{'' 
coveragePoints(( 
[(( 

sourceLine(( )
](() *
.((* +
Add((+ .
(((. /
visited((/ 6
)((6 7
;((7 8
})) 
else** 
{++ 
coveragePoints,, 
.,, 
Add,, "
(,," #

sourceLine,,# -
,,,- .
new,,/ 2
CoveragePoint,,3 @
(,,@ A

sourceLine,,A K
,,,K L
visited,,M T
),,T U
),,U V
;,,V W
}-- 
}.. 	
public00 
void00 
AddBranchPoint00 "
(00" #
string00# )

sourceLine00* 4
,004 5
string006 <
visitedCount00= I
)00I J
{11 	
Boolean22 
branchVisited22 !
=22" #
int22$ '
.22' (
Parse22( -
(22- .
visitedCount22. :
)22: ;
>22< =
$num22> ?
?22@ A
true22B F
:22G H
false22I N
;22N O
IBranchPoint33 
branchPoint33 $
=33% &
new33' *
BranchPoint33+ 6
(336 7
branchVisited337 D
)33D E
;33E F
branchPoints44 
[44 

sourceLine44 #
]44# $
=44% &
branchPoints44' 3
.443 4
ContainsKey444 ?
(44? @

sourceLine44@ J
)44J K
?44L M
branchPoints44N Z
[44Z [

sourceLine44[ e
]44e f
.44f g
Add44g j
(44j k
branchPoint44k v
)44v w
:44w x
branchPoint	44y „
;
44„ …
}66 	
public88 
IBranchPoint88 
BranchPoint88 '
(88' (
string88( .

sourceLine88/ 9
)889 :
{99 	
return:: 
branchPoints:: 
.::  
ContainsKey::  +
(::+ ,

sourceLine::, 6
)::6 7
?::7 8
branchPoints::8 D
[::D E

sourceLine::E O
]::O P
:::P Q
null::Q U
;::U V
};; 	
}<< 
}== ¿
mE:\users\stevpet\my documents\visual studio 2015\Projects\OpenCover2Generic\OpenCover2Generic\IBranchPoint.cs
	namespace 	
BHGE
 
. 
	SonarQube 
. 
OpenCover2Generic *
{ 
internal 
	interface 
IBranchPoint #
{ 
int 
Paths 
{ 
get 
; 
} 
int 
PathsVisited 
{ 
get 
; 
}  !
IBranchPoint 
Add 
( 
IBranchPoint %
branchPoint& 1
)1 2
;2 3
} 
}		 ·
sE:\users\stevpet\my documents\visual studio 2015\Projects\OpenCover2Generic\OpenCover2Generic\ICommandLineParser.cs
	namespace 	
BHGE
 
. 
	SonarQube 
. 
OpenCover2Generic *
{ 
	interface		 
ICommandLineParser		  
{

 
string 
[ 
] 
Args 
{ 
get 
; 
set !
;! "
}# $
string 
OpenCoverPath 
( 
) 
; 
string 
GenericPath 
( 
) 
; 
} 
} ¤
kE:\users\stevpet\my documents\visual studio 2015\Projects\OpenCover2Generic\OpenCover2Generic\IConverter.cs
	namespace 	
BHGE
 
. 
	SonarQube 
. 
OpenCover2Generic *
{ 
internal 
	interface 

IConverter !
{ 
void 
Convert 
( 
StreamWriter !
writer" (
,( )
StreamReader* 6
reader7 =
)= >
;> ?
} 
}		 °
oE:\users\stevpet\my documents\visual studio 2015\Projects\OpenCover2Generic\OpenCover2Generic\ICoveragePoint.cs
	namespace 	
BHGE
 
. 
	SonarQube 
. 
OpenCover2Generic *
{ 
internal 
	interface 
ICoveragePoint %
{ 
int 

SourceLine 
{ 
get 
; 
} 
bool 
Covered 
{ 
get 
; 
} 
void 
Add 
( 
bool 
visited 
) 
;  
}		 
}

 „	
sE:\users\stevpet\my documents\visual studio 2015\Projects\OpenCover2Generic\OpenCover2Generic\IFileCoverageModel.cs
	namespace 	
BHGE
 
. 
	SonarQube 
. 
OpenCover2Generic *
{ 
	interface 
IFileCoverageModel  
{ 
string 
FullPath 
{ 
get 
; 
}  
void		 
AddSequencePoint		 
(		 
string		 $

sourceLine		% /
,		/ 0
string		0 6
visitedCount		7 C
)		C D
;		D E
IList

 
<

 
ICoveragePoint

 
>

 
SequencePoints

 ,
{

- .
get

/ 2
;

2 3
}

4 5
void 
AddBranchPoint 
( 
string "

sourceLine# -
,- .
string/ 5
visitedCount6 B
)B C
;C D
IBranchPoint 
BranchPoint  
(  !
string! '

sourceLine( 2
)2 3
;3 4
} 
} ã
kE:\users\stevpet\my documents\visual studio 2015\Projects\OpenCover2Generic\OpenCover2Generic\IGenerator.cs
	namespace 	
BHGE
 
. 
	SonarQube 
. 
OpenCover2Generic *
{ 
public 

	interface 

IGenerator 
{ 
} 
} Ü	
gE:\users\stevpet\my documents\visual studio 2015\Projects\OpenCover2Generic\OpenCover2Generic\IModel.cs
	namespace 	
BHGE
 
. 
	SonarQube 
. 
OpenCover2Generic *
{ 
	interface 
IModel 
{ 
void 
AddFile 
( 
string 
fileId "
," #
string$ *
filePath+ 3
)3 4
;4 5
IList 
< 
IFileCoverageModel  
>  !
GetCoverage" -
(- .
). /
;/ 0
void		 
AddSequencePoint		 
(		 
string		 $
fileId		% +
,		+ ,
string		- 3

sourceLine		4 >
,		> ?
string		@ F
visitedCount		G S
)		S T
;		T U
void

 
AddBranchPoint

 
(

 
string

 "
fileId

# )
,

) *
string

+ 1

sourceLine

2 <
,

< =
string

> D
visitedCount

E Q
)

Q R
;

R S
void 
Init 
( 
) 
; 
} 
} Û
tE:\users\stevpet\my documents\visual studio 2015\Projects\OpenCover2Generic\OpenCover2Generic\ISourceFileCoverage.cs
	namespace 	
BHGE
 
. 
	SonarQube 
. 
OpenCover2Generic *
{ 
internal 
	interface 
ISourceFileCoverage *
{ 
string 
Path 
{ 
get 
; 
} 
} 
} Ì
fE:\users\stevpet\my documents\visual studio 2015\Projects\OpenCover2Generic\OpenCover2Generic\Model.cs
	namespace 	
BHGE
 
. 
	SonarQube 
. 
OpenCover2Generic *
{		 
internal

 
class

 
Model

 
:

 
IModel

 !
{ 
private 

Dictionary 
< 
string !
,! "
IFileCoverageModel# 5
>5 6
sourceFiles7 B
;B C
public 
Model 
( 
) 
{ 	
Init 
( 
) 
; 
} 	
public 
void 
AddFile 
( 
string "
fileId# )
,) *
string+ 1
filePath2 :
): ;
{ 	
sourceFiles 
. 
Add 
( 
fileId "
," #
new# &
FileCoverageModel' 8
(8 9
filePath9 A
)A B
)B C
;C D
} 	
public 
void 
AddSequencePoint $
($ %
string% +
fileId, 2
,2 3
string4 :

sourceLine; E
,E F
stringG M
visitedCountN Z
)Z [
{ 	
sourceFiles 
[ 
fileId 
] 
.  
AddSequencePoint  0
(0 1

sourceLine1 ;
,; <
visitedCount= I
)I J
;J K
} 	
public 
IList 
< 
IFileCoverageModel '
>' (
GetCoverage) 4
(4 5
)5 6
{ 	
return 
sourceFiles 
. 
Values %
.% &
ToList& ,
(, -
)- .
;. /
} 	
public!! 
void!! 
AddBranchPoint!! "
(!!" #
string!!# )
fileId!!* 0
,!!0 1
string!!2 8

sourceLine!!9 C
,!!C D
string!!E K
visitedCount!!L X
)!!X Y
{"" 	
sourceFiles## 
[## 
fileId## 
]## 
.##  
AddBranchPoint##  .
(##. /

sourceLine##/ 9
,##9 :
visitedCount##; G
)##G H
;##H I
}$$ 	
public&& 
void&& 
Init&& 
(&& 
)&& 
{'' 	
sourceFiles(( 
=(( 
new(( 

Dictionary(( (
<((( )
string(() /
,((/ 0
IFileCoverageModel((1 C
>((C D
(((D E
)((E F
;((F G
})) 	
}** 
}++ ‹
hE:\users\stevpet\my documents\visual studio 2015\Projects\OpenCover2Generic\OpenCover2Generic\Program.cs
	namespace 	
BHGE
 
. 
	SonarQube 
. 
OpenCover2Generic *
{		 
class

 	
Program


 
{ 
static 
void 
Main 
( 
string 
[  
]  !
args" &
)& '
{ 	
var 
commandLineParser !
=" #
new$ '
CommandLineParser( 9
(9 :
): ;
;; <
var 
model 
= 
new 
Model !
(! "
)" #
;# $
var 
	converter 
= 
new 
	Converter  )
() *
model* /
)/ 0
;0 1
commandLineParser 
. 
Args "
=# $
args% )
;) *
string 
openCoverPath  
=! "
commandLineParser# 4
.4 5
OpenCoverPath5 B
(B C
)C D
;D E
string 
genericPath 
=  
commandLineParser! 2
.2 3
GenericPath3 >
(> ?
)? @
;@ A
var 

fileWriter 
= 
new  
StreamWriter! -
(- .
genericPath. 9
)9 :
;: ;
var 

fileReader 
= 
new  
StreamReader! -
(- .
openCoverPath. ;
); <
;< =
	converter 
. 
Convert 
( 

fileWriter (
,( )

fileReader* 4
)4 5
;5 6
} 	
} 
} ´
xE:\users\stevpet\my documents\visual studio 2015\Projects\OpenCover2Generic\OpenCover2Generic\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str ,
), -
]- .
[		 
assembly		 	
:			 

AssemblyDescription		 
(		 
$str		 !
)		! "
]		" #
[

 
assembly

 	
:

	 
!
AssemblyConfiguration

  
(

  !
$str

! #
)

# $
]

$ %
[ 
assembly 	
:	 

AssemblyCompany 
( 
$str 6
)6 7
]7 8
[ 
assembly 	
:	 

AssemblyProduct 
( 
$str .
). /
]/ 0
[ 
assembly 	
:	 

AssemblyCopyright 
( 
$str I
)I J
]J K
[ 
assembly 	
:	 

AssemblyTrademark 
( 
$str 
)  
]  !
[ 
assembly 	
:	 

AssemblyCulture 
( 
$str 
) 
] 
[ 
assembly 	
:	 


ComVisible 
( 
false 
) 
] 
[ 
assembly 	
:	 

Guid 
( 
$str 6
)6 7
]7 8
[## 
assembly## 	
:##	 

AssemblyVersion## 
(## 
$str## $
)##$ %
]##% &
[$$ 
assembly$$ 	
:$$	 

AssemblyFileVersion$$ 
($$ 
$str$$ (
)$$( )
]$$) *
[%% 
assembly%% 	
:%%	 

InternalsVisibleTo%% 
(%% 
$str%% ;
)%%; <
]%%< =