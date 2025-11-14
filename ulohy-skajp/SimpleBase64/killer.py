import sys
import os

user_answer = sys.argv[1]

if user_answer == "B4S3":
    # Run cd ../ulohy-skajp/SimpleBase64 && docker compose down -v
    os.system("cd ../ulohy-skajp/SimpleBase64 && docker compose down -v")
    print("true")
else:
    print("false")