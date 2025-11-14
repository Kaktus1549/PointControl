FROM debian:latest

RUN apt-get update && apt-get install -y sudo openssh-server

# Create a user and set a password
RUN useradd -m user && echo "user:heslo" | chpasswd
RUN useradd -m skajp && echo "skajp:heslo" | chpasswd
# Allow user to run specific sudo command without password
RUN echo "skajp  ALL=(ALL:ALL) ALL" >> /etc/sudoers

# Add the flag
RUN echo "Password is: H1DD3N" > /home/user/.flag.txt


RUN echo "\n----- AIRSPORT -----\nUser: user\nPassword: heslo\nFlag location: Unkown\nGoal: Get the flag contents\n--------------------\n" > /etc/ssh/banner.txt

# Allow password authentication for SSH
RUN sed -i 's/#PasswordAuthentication yes/PasswordAuthentication yes/' /etc/ssh/sshd_config
RUN echo "banner /etc/ssh/banner.txt" >> /etc/ssh/sshd_config

RUN ln -sf /bin/bash /bin/sh

# Expose SSH port
EXPOSE 22

# Start SSH service and keep container running
CMD service ssh start && tail -f /dev/null
