@echo off

cd "%~dp1"
%~d1

echo *******************************************************************************
echo *
echo *              ��Ƶתflv���ĵ�תswf
echo *
echo *    ��ͬһĿ¼����һ��ͬ����.swf��.flv�ļ���
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
ENDLOCAL

@echo off
ping -n 2 127.0.0.1 > nul
