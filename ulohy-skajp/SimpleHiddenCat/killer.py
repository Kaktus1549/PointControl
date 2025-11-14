import sys
import os

user_answer = sys.argv[1]

os.system("cd ../ulohy-skajp/SimpleHiddenCat && docker compose down -v")


if user_answer.lower() == "h1dd3n":
    # Run cd ../ulohy-skajp/SimpleHiddenCat && docker compose down -v
    print("true")
else:
    print("false")