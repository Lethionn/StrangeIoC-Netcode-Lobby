# **StrangeIoC Netcode Lobby**

This is the lobby system used in the multiplayer game project *Slay the Party*. It is built using Unity's **Netcode for GameObjects** and **Lobby** packages, integrated with the **StrangeIoC framework**. The system utilizes a host-client model, enabling players to create or join lobbies for gameplay. *(Note: The game itself is not included.)*

## **Features**

### **Menu Features**
1. **Quick Game**  
   - Automatically finds and joins an available lobby.  
   - If no lobbies are available, creates a new one.  

2. **Create Lobby**  
   - Allows players to create either public or private lobbies.  
   - Private lobbies are excluded from the *Quick Game* search.  

3. **Join Lobby**  
   - Lets players join a specific lobby using a unique lobby code.  

4. **Player Name**  
   - Enables players to choose a custom name.  
   - Assigns a random name on the first launch.

---

### **Lobby Features**
1. **Lobby Information**  
   - Displays details such as:
     - **Lobby code**
     - **Host name**
     - **Public/private status**
     - **Game start requirements**  

2. **Ready Check**  
   - Players can mark themselves as ready.  
   - The host can only start the game once all players are ready.

3. **Kick Functionality**  
   - The lobby owner can remove unwanted players.  
   - Kicked players cannot rejoin the same lobby.

---

## **Preview**

- 📹 **[Watch a quick video overview](https://www.youtube.com/watch?v=Aw7e6vfr9uM)**  
- 📂 **[Follow the full project here](https://www.youtube.com/@ozgrsrgz)**  

---

### **How to Use**
1. Clone the repository:
   ```bash
   git clone https://github.com/your-repo/StrangeIoC-Netcode-Lobby.git

