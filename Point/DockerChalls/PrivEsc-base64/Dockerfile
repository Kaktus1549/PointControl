FROM ubuntu:latest

RUN apt-get update && apt-get install -y sudo openssh-server

# Create a user and set a password
RUN useradd -m user && echo "user:heslo" | chpasswd
RUN useradd -m administrator && echo "administrator:heslo123" | chpasswd
# Allow user to run specific sudo command without password
RUN echo "user ALL = NOPASSWD: /usr/bin/base64" >> /etc/sudoers
RUN echo "administrator ALL=(ALL:ALL) ALL" >> /etc/sudoers

# Add the flag
RUN echo "CCT{w3ll_th4t_w4snt_h4rd}" > /root/flag.txt

# Set up SSH server
RUN mkdir /var/run/sshd

RUN echo "\n----- PointControl -----\nUser: user\nPassword: heslo\nFlag location: /root/flag.txt\nGoal: Get the flag contents\n------------------------\n" > /etc/ssh/banner.txt

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
