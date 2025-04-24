#!/bin/bash

arg1=$1

if [ -z "$arg1" ]; then
  echo "No argument provided"
  exit 1
fi

if [ "$arg1" == "test" ]; then
  echo "true"
else
  echo "false"
fi