# Contribution Guide
Before
contributing, please read the [code of conduct](https://github.com/episerver/Foundation/blob/develop/docs/code-of-conduct.md).  Contributions are greatly appreciated via forks. Episerver retains control of the direction of the project and reserves the right to close issues and PR:s that don’t align with the project roadmap.


* First, if you found a bug yourself you need to report it and let it as an issue in Github.
* Then, assign it you.
* Read through this document to make sure you follow the practices for developing on Foundation.
* __Test__, code, __test__, code, __test__
* Create a pull request with your changes, squash your changes to a single commit (unless multiple commits makes sense for some reason)

---

## Changing code in Foundation

* Create branch off develop named XXX-shortname where XXX is the issue number in Github, for example "bugfix/1231-thumbnails-for-media". See branching chapter for details.
* For bug fixes add a unit test to catch the bug before continuing
* Write tests, code, write tests, code  :)
* Make sure you have the latest code from develop
* Write a commit message according to the commit message guidelines below
* Make sure builds are green, Foundation CI runs on every checkin
* Create a pull requests form your branch to develop and add reviewer if you know who should review, otherwise leave blank

---

## Before submitting a pull-request
* Make sure the code is documented
* Make sure the tests are up to date and test the new code
* Make sure the correct issue ids are present in the commit message
* Follow the commit message guideline

---

## Accepting pull requests

Pull request are the official code review where someone on the team signs off on the code. It is encouraged to do code reviews continuously before you push to the repository but this is not enforced and up to every developer to choose to do so.

All developers are encourage to read, review, and comment on pull requests to make sure code reviews are a collaborate effort.

When merging, always delete source branch.

---

## Branching model in Foundation

We are using the workflow as described in https://www.atlassian.com/git/workflows#!workflow-gitflow with some minor modifications.

### Master branch

Should always contain tested, working, releasable code. You can only get code onto master by creating pull requests that then needs to be reviewed and accepted.

### Develop branch

Acts as integration branch for feature and bugfix branches that should go into the next release. You can only get code onto master by creating pull requests that then needs to be reviewed and accepted. Should only contain completely implemented work items.

### Feature branches

Created from develop and should be named feature/`<issue id in github>-<short description>`. For example to work on "User Story 35: Remove the 'Classic' link stage in the API and only use permanent links" you would create a branch from develop named "feature/35-remove-classic-links". Note that the `<short description>` is all lower-case with hyphens.

Merge to the develop branch by creating a pull request.

### Bugfix branches

Created from develop and should be named bugfix/`<issue id in github>-<short description>`. For example to work on "Bug 111571: MVC rendering of built-in properties" you should create a branch named something like "bugfix/111571-mvc-rendering-properties". Note that the `<short description>` is all lower-case with hyphens.

Merge to the develop branch by creating a pull request.

---

## Commit Message Guidelines ##

To make the history easier to read and the changes easier to understand a commit message should be created according to the rules below, which are a slightly modified version of [these seven rules](http://chris.beams.io/posts/git-commit/#seven-rules).

* Separate subject from body with a blank line
* Limit the subject to 50 characters
* Capitalize the subject line
* Do not end the subject line with a period
* Wrap the body at 72 characters
* Use the body to explain what and why vs. how
* Add a reference to the associated task

**Example**  
Handle multiple errors from the server  

Extended the message view model to have additional information which  
will be displayed beneath the error message in the alert dialog. Used  
the additional information in the mark as ready to publish method to  
handle errors that may occur when dealing with multiple items.  

