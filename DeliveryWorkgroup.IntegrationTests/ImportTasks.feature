Feature: ImportTasks

@mytag
Scenario : someproject
Given new project file
When I import "TasksImport" with sheet "Sheet1"
Then my project has 5 tasks
And  task "1" is "task1"
And  task "2" is "task2"