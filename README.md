# Diff test

A small api project to present some of my knowledge.

## Description

Provides 2 http endpoints (<host>/v1/diff/<ID>/left and <host>/v1/diff/<ID>/right) that accept 
JSON containing base64 encoded binary data on both endpoints. 
* The provided data needs to be diff-ed and the results shall be available on a third endpoint 
(<host>/v1/diff/<ID>). The results shall provide the following info in JSON format: 
* If equal return that 
* If not of equal size just return that 
* If of same size provide insight in where the diff are, actual diffs are not needed. 
  * So mainly offsets + length in the data 


## Getting Started


### Executing program

* using terminal in the `DiffProject\DiffProject.Api` folder type `dotnet run` 
* use postman or curl for put/get commands
  * to insert data
	```
	GET /v1/diff/1/left
	{ 
		"data": "AAAAAA==" 
	} 
	```
	```
	GET /v1/diff/1/left
	{ 
		"data": "AABAAA==" 
	} 
	```

  * to get diff:
	```
	GET /v1/diff/1 
	```

## Authors

* Me
* StackOwerflow :)
* Udemy

## Unnecessary but I want it here

https://xkcd.com/242

https://www.youtube.com/watch?v=y8OnoxKotPQ

