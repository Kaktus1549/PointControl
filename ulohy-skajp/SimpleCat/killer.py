import sys
import os

user_answer = sys.argv[1]

if user_answer == "C4T":
    # Run cd ../ulohy-skajp/SimpleCat && docker compose down -v
    os.system("cd ../ulohy-skajp/SimpleCat && docker compose down -v")
    print("true")
else:
    print("false")