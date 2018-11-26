## Project structure

#### Project for both challenges

There are two challenges in the coding challenge document. This repository includes all answers with each challenges in their own project respectively. `Challenge1` is the project for the first challenge (online message server), and `Challenge1.Tests` will be the test project with unit tests that guarantee its correctness. `Challenge2` is the project for the second challenge (buying gifts), and `Challenge2.Tests` will be the test project with unit tests that guarantee its correctness. Code for bonus question for challenge 2 is also included.

#### Batteries included

To make it easier to getting started, each project includes one or several files starting with `docker-compose*` for use with `docker-compose` commands. It will automatically pull required packages, build the project and run the project either as a local server listening on port `5000` (for challenge 1), or give the user an interface for them to test out their gifting skills (`find-pair.sh` and `find-trio.sh` and sample file input `prices.txt` is also included).

## Challenge 1

#### How to start

##### Build the project

Building the project requires no special configuration thanks to `docker`. It will automatically pull required containers and dependencies and build the project. The final output files are in the `Docker/publish` folder. 

To build the project, please run:

`docker-compose -f docker-compose.build.yml run --rm build`

##### Start the server

To start the server, please run:

`docker-compose -f docker-compose.yml run --service-ports --rm paxos-api`

The server will start listening to all incoming requests on `localhost` and port `5000`. Please use any browser to open `http://localhost:5000` to access the `Swagger UI`, which includes a user-friendly documentation of the APIs, and also ways to test them. If the user prefer testing the APIs with other tools, like `Postman`, they can direct all requests to corresponding endpoints (`/messages` and `/messages/<hash>`).

#### How to test

Testing the project is also very easy. Simply run:

`docker-compose -f docker-compose.test.yml run --rm test`

#### Why do you include `data.db`

Usually no binary file should be committed to a VCS. However, for testing purposes I included `data.db` in the repository. (Same goes for the `DATABASE_CONNECTION_STRING` in the `docker-compose.yml` file, which should always been kept as an environmental variable).

#### Performance question

Q: What would be the bottleneck(s) be in your implementation as you acquire more users? How you might scale your micro service?

A: There are multiple ways to affect the performance of the service.

##### More read and less generate

In the case where more read operation is performed than generating the hash, the service could suffer from the performance of the database. There are multiple ways to solve this issue. First, since there are a lot of read operation, caching can be a great way to reduce such latency. Caches can be put in different parts in the system, e.g. it can be put in between front-end and back-end, or between back-end and database. Second, if the number of the read operation keeps growing, partitioning (sharding) can be of great help. 

##### More generate and less read

In the case where more hash is generated than being read, the service could suffer from the performance of the computing resources (and also likely the performance of the database). Aside from increasing the computing power (scale up) and partitioning the input messages and delegate them to different machines (scale out), a queueing solution can also be used to decrease the latency of the requests. For solving the performance issue introduced by the database, a cache (preferably write-back) should be placed in between the back-end system and database.

## Challenge 2

#### How to start

##### Build the project

Building the project requires no special configuration thanks to `docker`. It will automatically pull required containers and dependencies and build the project. The final output files are in the `Docker/publish` folder. 

To build the project, please run:

`docker-compose -f docker-compose.build.yml run --rm build`

##### Start the interactive interface

To start the interactive interface and start testing the program, please run:

`docker-compose -f docker-compose.yml run --rm paxos /bin/bash`

The interactive interface allows user to start testing the `find-pair` and `find-trio` program. 

Usage:

`find-pair <input-file> <giftcard-value>`

`find-trio <input-file> <giftcard-value>`

##### Bring your own input files

All input files should be put in the `data` folder before start the interactive interface for the program to be able to read. However, user should not prefix the `input-file` with `data/` as it is implied, only the name of the file is required, e.g. `prices.txt`. The folder mapping can also be changed by altering the configuration in `docker-compose.yml`.

#### How to test

Testing the project is also very easy. Simply run:

`docker-compose -f docker-compose.test.yml run --rm test`

#### Performance analysis

Actual methods that solve the problems can be found inside `Program.cs`. `GetItems()` method is used for the program to read and parse the input file. It reads the file as a stream and retrieve data line by line for parsing, so it will not need to load the entire file into memory before doing anything with it. Since the input file includes all entries already sorted by the price, the `GetItems()` method will smartly only read all items that has a price lower than the gift card value so no data will be load into the memory if they are for sure not going to be picked. 

`FindPair()` method is used to find two items that can be bought with the given gift card value. 

- Time Complexity: `O(N)`, `N` being the count of the items

- Space Complexity: `O(1)`

`FindTrio()` method is used to find three items that can be bought with the given gift card value.

- Time Complexity: `O(N^2)`, `N` being the count of the items
- Space Complexity: `O(1)`

