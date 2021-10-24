cd /d %~dp0
SET BASE_PATH=%~dp0
SET CONDA_PATH=%~dp0python
set INSTALLER=%BASE_PATH%Miniconda3-latest-Windows-x86_64.exe

IF not EXIST %INSTALLER% (
	bitsadmin /TRANSFER miniconda  https://repo.anaconda.com/miniconda/Miniconda3-py38_4.9.2-Windows-x86_64.exe %INSTALLER%
)

IF not EXIST %CONDA_PATH%\python.exe (
	mkdir %CONDA_PATH%
	%INSTALLER% /S /InstallationType=JustMe /AddToPath=0 /RegisterPython=0  /NoRegistry=1 /D=%CONDA_PATH%
)

call env.bat

call conda update -y -n base -c defaults

call conda update -y --all

pip install pythonnet opencv-python pillow tqdm seaborn

pip cache purge