@echo off
SETLOCAL ENABLEDELAYEDEXPANSION
REM ###################################################################################################
REM # ABOUT : Build script to run t4templates outside of visual studio
REM # The templates generate the convenience properties for the IoC 
REM # container to easily access various services.
REM #
REM # Arguments: The following arguments must be supplied to this file
REM # index,	variable,		example					description
REM # arg1 : 	%1				c:\code\Brightline		Used to replace $(SolutionDir) in t4template
REM # arg1 : 	%2				bin\Debug				Used to replace $(OutDir) in t4template
REM # arg1 : 	%3				gen.cs					Used as suffix for auto-generated t4template
REM ###################################################################################################

:: set the working dir (default to current dir)
set wdir=%cd%
if not (%1)==() set wdir=%1

REM Capture the input arguments and declare some variables.
set PROJDIR=%1
set OUTDIR=%2
set BLHOME=!PROJDIR:~0,-17!
set NANT=%BLHOME%tools\nant\0.9.2\nant.exe

echo work dir: %wdir%
echo blhome is : %BLHOME%
echo proj is : %PROJDIR%
echo out dir : %OUTDIR%
echo nant dir: %NANT%

REM 2. Replace the macros inside the template file with the actual values 
REM OutDir=bin\Debug SolutionDir=C:\Code\BrightLine\
%NANT% -buildfile:%BLHOME%buildscripts\transformt4files.build -D:solutiondir=%PROJDIR% -D:outdir=%OUTDIR%