RPPBranchedMigrator
===================

This project is inspired by Sean Chambers' excellent FluentMigrator. It's is intended to fill in some of the small gaps in that otherwise excellent tool. Namely, that developers working on concurrent braches sometimes find there are clashes in naming conventions for the SQL scripts they create, leading to branches not being able to be Merged automagically and files having to be renamed at Pull Request time. It's being developed as a new project, partly as a result of the different architectural style that's had to be taken to cater for concurrent branches (using a time-based rather than a sequentially-numbered based approach to migration scripts), and partly because I wanted to have the fun and deep understanding of the topic that only really comes from starting from scratch and making mistakes all on your own.

This project may be downloaded, distributed, copied, forked and improved as users see fit, subject to the standard GNU general public licence, a copy of which may be found at: https://www.gnu.org/licenses/gpl


Rachel P Pierson,
March 2013
