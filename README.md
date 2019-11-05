# Cosmic Quest - Order and Chaos
This is the repository for *VACATe Game Studios'* capstone project titled *Cosmic Quest - Order and Chaos*. This repository contains all the code for the project, as well as a wiki containing all of the documentation and deliverables.

## Development

### Getting Started

To get started with development, you must have the following dependencies installed:
```
Unity Editor 2019.2.8f1
git
```

In order for git to be able to handle some of the large game files, you must install the Git Large File Storage plugin from the [git-lfs site](https://git-lfs.github.com/). Once you have this installed, go into the root directory of this repository on your computer and run `git lfs install` to setup the proper Git hooks. This only needs to be done once.

### Git Development Strategy

Each developer must work on their own branch (or series of branches), and open a pull request in order to merge their branch to master. You will not be able to push directly to master.

Here is an example workflow:
``` bash
# Create a branch from the current branch
$ git branch my-branch

# Checkout your new branch
$ git checkout my-branch

# Push changes on your branch
$ git push origin my-branch

# Pull the latest changes from master to your branch (may cause merge issues)
$ git pull origin master
```
