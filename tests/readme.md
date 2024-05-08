# Manual tests with curl:

curl -X POST -H "Content-Type: application/json" -d @tests/test.json http://localhost:5001/predict

curl -F "file=@tests/test_file_contents.cpp" -F "model=codebert" -F "vulnerability=input_validation" -X POST http://localhost:5001/predict