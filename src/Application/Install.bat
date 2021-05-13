cd /d %1

nssm install %2 %1\Run.bat %1
nssm set %2 Description "Runs the micro-service"
nssm set %2 AppStdout "%1\log-stdout.txt"
nssm set %2 AppStderr "%1\log-stderr.txt"
nssm set %2 AppDirectory %1
nssm start %2