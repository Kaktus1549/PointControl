import sys
import os

user_answer = sys.argv[1]

os.system("cd ../ulohy-skajp/SimpleBase64 && docker compose down -v")


if user_answer.lower() == "b4s3":
    # Run cd ../ulohy-skajp/SimpleBase64 && docker compose down -v
    print("true")
else:
    print("false")