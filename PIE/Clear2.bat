REM start to clean
echo Start to clean all bin and obj folder under Noodle repo
@echo on
@for /d /r %%c in (obj) do @if exist "%%c" (@rd /s /q "%%c" & echo Delete %%c)
@for /d /r %%c in (bin) do @if exist "%%c" (@rd /s /q "%%c" & echo Delete %%c)
pause
