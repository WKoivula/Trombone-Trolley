# [DH2413 AGI : Trombone Trolley featuring Trolley Trombone]

![VR Banner](https://img.shields.io/badge/Platform-Meta_Quest_3-blue)
![Unity Version](https://img.shields.io/badge/Unity-2022.3_LTS-black?logo=unity)
![Render Pipeline](https://img.shields.io/badge/Render_Pipeline-URP-informational)

This is the repo for "Trombone Trolley featuring Trolley Trombone", A project for the DH2413 Advanced interaction and graphics course at KTH 2025. 
## 🛠 Technical Specifications

* **Unity Version:** `6000.2.10f1` 
* **XR Framework:** OpenXR with some Meta XR All-in-One SDK building blocks.  
* **Render Pipeline:** Universal Render Pipeline (URP)
* **Input System:** New Unity Input System

---

## 🚀 Getting Started

Project has only been tested on a Quest 3 and can only run via meta quest link setup (STEAMVR HAS NOT BEEN TESTED). The project CANNOT be built to the device and must be streamed via LINK (see important notes for more). 

### 1. Prerequisites

Before opening the project, ensure you have the following installed and configured:

* **Meta Quest Link App:** Installed on your PC ([Download here](https://www.meta.com/quest/setup/)).
* **Meta Quest 3 Headset:** With Developer Mode enabled via the Meta Horizon mobile app.
* **Link Cable:** A high-quality USB-C 3.0 cable (or a strong 5GHz Wi-Fi connection for Air Link).
* **Unity Hub:** To manage the editor version.

### 2. Installation

1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/yourusername/your-repo-name.git](https://github.com/yourusername/your-repo-name.git)
    ```
2.  **Open in Unity:**
    * Open Unity Hub.
    * Click **Add** -> **Add project from disk**.
    * Select the root folder of this repository.
    * Ensure the version is set to `6000.2.10f1`.

### 3. How to Run (Quest Link)


1.  **Connect your Quest 3** to your PC via the Link cable.
2.  Inside the headset, select **Enable Quest Link** from the quick settings menu.
3.  In the Unity Editor, go to `Edit > Project Settings > XR Plug-in Management`.
4.  Ensure **OpenXR** is checked under the **PC (Windows)** tab.
5.  Press **Play** in the Unity Editor. The view should now appear inside your headset.

---

## 🎮 Controls

Player cannot move by themselves. Instead the cart moves when spacebar is pressed. Only relevant interaction is grabbing the trombone with hands.  

---

## ⚠️ Important Notes

* **Project not buildable** The project cannot be built on the headset due to the project making use of LASP ([Link to LASP]([https://www.meta.com/quest/setup/](https://github.com/keijiro/Lasp))). Therefore you must run the project via meta quest LINK and stream the project, I.e PCVR.
* **Controllers Not tested/implemented:** Project makes of hand tracking only. To calibrate the trombones position the "C" key is used while Spacebar is used to start the game. When the game ends, you simply exit the game and rerun it in the editor. The use of controllers is not supported. 
* **Basic scene** The game exists inside the "BasicScene" scene with the remaining scenes being for testing or outdated. 


