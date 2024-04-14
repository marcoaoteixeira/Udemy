# Install ZSH

- `sudo apt-get install -y zsh`

## Set ZSH As Default Shell

- `chsh -s $(which zsh)`

Restart your terminal.

### How to Go Back To Bash

- `chsh -s $(which bash)`

Restart your terminal.

## Install Plugins

Create a directory named **.zsh** on your home directory.

- `mkdir .zsh`

## Installing Oh-My-ZSH

- Go to your **HOME** directory
  - Execute: `sh -c "$(curl -fsSL https://raw.github.com/ohmyzsh/ohmyzsh/master/tools/install.sh)"`

## Installing PowerLevel10K Plugin

- First, download and install these fonts:
  - [MesloLGS NF Regular.ttf](https://github.com/romkatv/powerlevel10k-media/raw/master/MesloLGS%20NF%20Regular.ttf)
  - [MesloLGS NF Bold.ttf](https://github.com/romkatv/powerlevel10k-media/raw/master/MesloLGS%20NF%20Bold.ttf)
  - [MesloLGS NF Italic.ttf](https://github.com/romkatv/powerlevel10k-media/raw/master/MesloLGS%20NF%20Italic.ttf)
  - [MesloLGS NF Bold Italic.ttf](https://github.com/romkatv/powerlevel10k-media/raw/master/MesloLGS%20NF%20Bold%20Italic.ttf)
- Go to your Terminal / Profiles
  - Select the Profile that you're using
  - Go to Appearance and select **MesloLGS NF** as your font type.
- Make sure you're on your **HOME** directory: `cd ~`
- Clone this repository
  - `git clone --depth=1 https://github.com/romkatv/powerlevel10k.git ${ZSH_CUSTOM:-$HOME/.oh-my-zsh/custom}/themes/powerlevel10k`
- Execute these lines:
  - `echo 'source ~/.oh-my-zsh/custom/themes/powerlevel10k/powerlevel10k.zsh-theme' >> ~/.zshrc`
  - Open your `~/.zshrc` file and change the content of **ZSH_THEME**
    - `ZSH_THEME="powerlevel10k/powerlevel10k"`
  - Execute: `exec zsh`
    - This will start PowerLevel10K configuration. Just go through it and select what your hearts desires.

## Installing ZSH AutoSuggestion Plugin

- Go to `~/.zsh` directory
  - Clone ZSH AutoSuggestion repository
    - `git clone https://github.com/zsh-users/zsh-autosuggestions.git`
  - Execute command:
    - `echo 'source ~/.zsh/zsh-autosuggestions/zsh-autosuggestions.zsh' >> ~/.zshrc`
- Restart your terminal

## Installing ZSH Syntax Highlighting Plugin

- Go to `~/.zsh` directory
  - Clone ZSH AutoSuggestion repository
    - `https://github.com/zsh-users/zsh-syntax-highlighting.git`
    - Execute `echo "source ${(q-)PWD}/zsh-syntax-highlighting/zsh-syntax-highlighting.zsh" >> ${ZDOTDIR:-$HOME}/.zshrc`
  - Execute command:
    - `echo 'source ~/.zsh/zsh-syntax-highlighting/zsh-syntax-highlighting.zsh' >> ~/.zshrc`
- Restart your terminal

## Installing NVM for Linux

- Go to your **HOME** directory
- Execute
  - `curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.7/install.sh | bash`
- This will download and install **nvm**, after finish installation, execute:
  - `source ~/.zshrc`

## Installing .NET for Linux

- Go to your **HOME** directory
- Execute
  - `sudo apt-get install -y dotnet-sdk-8.0`

## Installing Python for Linux

- Go to your **HOME** directory
- Execute
  - `sudo apt-get install -y python3`
