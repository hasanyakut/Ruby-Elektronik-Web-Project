@echo off
echo ========================================
echo    Ruby Elektronik Microservices
echo ========================================
echo.

echo Starting ProductService...
start "ProductService" cmd /k "cd ProductService && dotnet run"

echo Starting OrderService...
start "OrderService" cmd /k "cd OrderService && dotnet run"

echo Starting UserService...
start "UserService" cmd /k "cd UserService && dotnet run"

echo Starting ServiceService...
start "ServiceService" cmd /k "cd ServiceService && dotnet run"

echo Starting Frontend...
start "Frontend" cmd /k "cd frontend && dotnet run"

echo.
echo ========================================
echo    Services Starting...
echo ========================================
echo.
echo ProductService: https://localhost:7001
echo OrderService:   https://localhost:7002  
echo UserService:    https://localhost:7003
echo ServiceService: http://localhost:7004
echo Frontend:       https://localhost:5001
echo.
echo Swagger UI:
echo - ProductService: https://localhost:7001/swagger
echo - OrderService:   https://localhost:7002/swagger
echo - UserService:    https://localhost:7003/swagger
echo - ServiceService: http://localhost:7004/swagger
echo.
echo Press any key to exit this window...
pause > nul
