#!/bin/bash

count=0
total=$(find DockerChalls -mindepth 1 -maxdepth 1 -type d | wc -l)

for f in DockerChalls/*; do
  if [ -d "$f" ]; then
    count=$((count + 1))
    cd "$f" || continue
    echo "\n\n[$count/$total] Starting docker in $f\n\n"
    docker compose up --build -d
    cd - > /dev/null
    echo "\n\n[$count/$total] $f is running\n\n"
    echo "-------------------------------"
  fi
done

