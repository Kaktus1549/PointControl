import sys
import os

user_answer = sys.argv[1]

os.system("cd ../ulohy-skajp/SimpleCat && docker compose down -v")


if user_answer.lower() == "c4t":
    # Run cd ../ulohy-skajp/SimpleCat && docker compose down -v
    print("true")
else:
    print("false")