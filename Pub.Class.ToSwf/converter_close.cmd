@echo off

cd "%~dp1"
%~d1

echo *******************************************************************************
echo *
echo *    %~dp1%~nx1
echo *    ����ת���ļ�����ȴ���������ҪһЩʱ��....

"%~dp0Pub.Class.ToSwf.exe" "%~dp1%~nx1"
if %ERRORLEVEL% == 0 (
	echo *
	echo *    �ļ�ת���ɹ���
	echo *
) else (
	echo *
	echo *    �ļ�ת��ʧ�ܣ�
	echo *
	goto End
)
goto End


:End
echo *******************************************************************************
echo.

