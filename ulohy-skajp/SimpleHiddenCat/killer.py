import sys
import os

user_answer = sys.argv[1]

if user_answer == "H1DD3N":
    # Run cd ../ulohy-skajp/SimpleHiddenCat && docker compose down -v
    os.system("cd ../ulohy-skajp/SimpleHiddenCat && docker compose down -v")
    print("true")
else:
    print("false")