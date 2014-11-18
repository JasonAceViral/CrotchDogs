#!/bin/sh
echo "\n"
if [ -n "$1" ]
then
    echo "Using directory: $1"
    cd "$1"
fi
if [ -z "$1" ]
then
echo "Using local directory."
fi
find . -name '*.meta' -type f -delete
echo "Meta files deleted. \n"
wait