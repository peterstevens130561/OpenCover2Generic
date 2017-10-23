Feature: Split OpenCover Coverage File into generic coverage files
	In order to be able to run automated tests in parallel
	As a developer
	I need each module to be collected in its own folder, from where I can later reassemble them into one coverage file

@MultipleModules
Scenario: A coverage file with two modules
	Given I have coverage file "TwoModules.xml"  for assembly "assembly1" 
	When I convert it
	Then I should have "Module1\Assembly1.xml"
	And I should have "Module2\Assembly1.xml"

@MultipleModules
Scenario: A coverage file with one module
	Given I have coverage file "OneModule.xml" for assembly "assembly1" 
	When I convert it
	Then I should have "Module1\Assembly1.xml"




