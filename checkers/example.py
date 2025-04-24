import sys

def Check(arguments: list) -> bool:
    #print(arguments) # ['example.py', 'argv1']

    if sys.argv[1] == r"CCT{example_flag}":
        return True
    else:
        return False

if __name__ == "__main__":
    """
    This is example script/structure of checkers for PointControl that will be played on Airsoft
    """
    print(Check(sys.argv))