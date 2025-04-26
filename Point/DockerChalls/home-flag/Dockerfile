FROM ubuntu:latest

RUN apt-get update && apt-get install -y sudo openssh-server

# Create a user and set a password
RUN useradd -m user && echo "user:heslo" | chpasswd
RUN useradd -m administrator && echo "administrator:heslo123" | chpasswd
# Allow user to run specific sudo command without password
RUN echo "user ALL=(ALL:ALL) ALL" >> /etc/sudoers
RUN echo "administrator ALL=(ALL:ALL) ALL" >> /etc/sudoers

# Add the flag
RUN echo "CCT{Sup3rDup3rS3cr3t}" > /home/user/.super_hidden_file

# Set up SSH server
RUN mkdir /var/run/sshd

RUN echo "\n----- PointControl -----\nUser: user\nPassword: heslo\nGoal: Get the flag contents from the hidden file inside your home folder\n------------------------\n" > /etc/ssh/banner.txt

# Allow password authentication for SSH
RUN sed -i 's/#PasswordAuthentication yes/PasswordAuthentication yes/' /etc/ssh/sshd_config
RUN echo "banner /etc/ssh/banner.txt" >> /etc/ssh/sshd_config
RUN chmod -x /etc/update-motd.d/*
RUN chmod +x /etc/update-motd.d/00-header
RUN rm /etc/legal

RUN ln -sf /bin/bash /bin/sh

# Expose SSH port
EXPOSE 22

# Start SSH service and keep container running
CMD service ssh start && tail -f /dev/null
