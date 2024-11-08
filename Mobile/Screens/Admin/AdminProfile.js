import React, { useState, useEffect } from 'react';
import { View, Text, TextInput, Image, TouchableOpacity, StyleSheet, Alert, Modal } from 'react-native';
import { Ionicons } from '@expo/vector-icons';
import AsyncStorage from '@react-native-async-storage/async-storage';

export default function AdminProfile({ route, navigation }) {
  const { setLoggedIn } = route.params; // Accessing setLoggedIn from route params
  const [adminData, setAdminData] = useState({
    firstname: "Shannon",
    lastname: "Kabotha",
    contactno: "123-456-7890",
    password: "Password123" // Placeholder password
  });

  const [isPasswordModalVisible, setPasswordModalVisible] = useState(false);
  const [isEditProfileModalVisible, setEditProfileModalVisible] = useState(false);
  const [newPassword, setNewPassword] = useState('');

  // Load profile data from AsyncStorage on mount
  useEffect(() => {
    const loadProfileData = async () => {
      try {
        const storedProfile = await AsyncStorage.getItem('adminProfile');
        if (storedProfile) {
          setAdminData(JSON.parse(storedProfile));
        }
      } catch (error) {
        console.log("Error loading profile data: ", error);
      }
    };
    loadProfileData();
  }, []);

  // Save profile data to AsyncStorage
  const saveProfileData = async (updatedData) => {
    try {
      await AsyncStorage.setItem('adminProfile', JSON.stringify(updatedData));
      setAdminData(updatedData);
      Alert.alert("Profile Updated", "Your profile has been updated successfully.");
    } catch (error) {
      console.log("Error saving profile data: ", error);
    }
  };

  // Handle Sign Out
  const SignOut = () => {
    Alert.alert("Logging Out", "You are now logged out");
    setLoggedIn(false);
    navigation.reset({
      index: 0,
      routes: [{ name: 'LoginPage' }],
    });
  };

  // Update Password
  const handlePasswordChange = () => {
    if (newPassword.length < 6) {
      Alert.alert("Error", "Password must be at least 6 characters.");
      return;
    }
    const updatedData = { ...adminData, password: newPassword };
    saveProfileData(updatedData);
    setPasswordModalVisible(false);
    Alert.alert("Success", "Password has been changed.");
  };

  // Update Profile (first name, last name, contact number)
  const handleProfileUpdate = (updatedData) => {
    saveProfileData(updatedData);
    setEditProfileModalVisible(false);
  };

  return (
    <View style={styles.container}>
      <Text style={styles.header}>Admin Profile</Text>
      <View style={styles.profileContainer}>
        <Image
          source={require('../../Resources/Margot 4k.jpg')}
          style={styles.profileImage}
        />
        <View style={styles.infoContainer}>
          <Text style={styles.label}>First Name:</Text>
          <View style={styles.inputContainer}>
            <Text style={styles.input}>{adminData.firstname}</Text>
          </View>

          <Text style={styles.label}>Last Name:</Text>
          <View style={styles.inputContainer}>
            <Text style={styles.input}>{adminData.lastname}</Text>
          </View>

          <Text style={styles.label}>Contact Number:</Text>
          <View style={styles.inputContainer}>
            <Text style={styles.input}>{adminData.contactno}</Text>
          </View>
        </View>
      </View>

      <View style={styles.buttonsContainer}>
        <TouchableOpacity style={styles.button} onPress={() => setPasswordModalVisible(true)}>
          <Ionicons name="lock-closed-outline" size={20} color="#FFFFFF" style={styles.icon} />
          <Text style={styles.buttonText}>Change Password</Text>
        </TouchableOpacity>

        <TouchableOpacity style={styles.button} onPress={() => setEditProfileModalVisible(true)}>
          <Ionicons name="settings-outline" size={20} color="#FFFFFF" style={styles.icon} />
          <Text style={styles.buttonText}>Edit Profile</Text>
        </TouchableOpacity>

        <TouchableOpacity style={styles.logoutButton} onPress={SignOut}>
          <Ionicons name="log-out-outline" size={20} color="#FFFFFF" style={styles.icon} />
          <Text style={styles.logoutButtonText}>Log Out</Text>
        </TouchableOpacity>
      </View>

      {/* Password Change Modal */}
      <Modal
        visible={isPasswordModalVisible}
        transparent={true}
        animationType="slide"
        onRequestClose={() => setPasswordModalVisible(false)}
      >
        <View style={styles.modalContainer}>
          <View style={styles.modalContent}>
            <Text style={styles.modalTitle}>Change Password</Text>
            <TextInput
              style={styles.modalInput}
              placeholder="Enter new password"
              secureTextEntry
              value={newPassword}
              onChangeText={setNewPassword}
            />
            <TouchableOpacity style={styles.modalButton} onPress={handlePasswordChange}>
              <Text style={styles.modalButtonText}>Save Password</Text>
            </TouchableOpacity>
            <TouchableOpacity style={styles.modalButton} onPress={() => setPasswordModalVisible(false)}>
              <Text style={styles.modalButtonText}>Cancel</Text>
            </TouchableOpacity>
          </View>
        </View>
      </Modal>

      {/* Edit Profile Modal */}
      <Modal
        visible={isEditProfileModalVisible}
        transparent={true}
        animationType="slide"
        onRequestClose={() => setEditProfileModalVisible(false)}
      >
        <View style={styles.modalContainer}>
          <View style={styles.modalContent}>
            <Text style={styles.modalTitle}>Edit Profile</Text>
            <TextInput
              style={styles.modalInput}
              placeholder="First Name"
              value={adminData.firstname}
              onChangeText={(text) => setAdminData({ ...adminData, firstname: text })}
            />
            <TextInput
              style={styles.modalInput}
              placeholder="Last Name"
              value={adminData.lastname}
              onChangeText={(text) => setAdminData({ ...adminData, lastname: text })}
            />
            <TextInput
              style={styles.modalInput}
              placeholder="Contact Number"
              value={adminData.contactno}
              onChangeText={(text) => setAdminData({ ...adminData, contactno: text })}
              keyboardType="phone-pad"
            />
            <TouchableOpacity
              style={styles.modalButton}
              onPress={() => handleProfileUpdate(adminData)}
            >
              <Text style={styles.modalButtonText}>Save Profile</Text>
            </TouchableOpacity>
            <TouchableOpacity
              style={styles.modalButton}
              onPress={() => setEditProfileModalVisible(false)}
            >
              <Text style={styles.modalButtonText}>Cancel</Text>
            </TouchableOpacity>
          </View>
        </View>
      </Modal>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#F6A1F8',
    padding: 15,
    alignItems: 'center',
  },
  header: {
    fontSize: 28,
    fontWeight: 'bold',
    marginBottom: 20,
    color: '#FFFFFF',
    letterSpacing: 1,
  },
  profileContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: 20,
    backgroundColor: '#FFF',
    borderRadius: 20,
    padding: 15,
    elevation: 3,
    width: '100%',
  },
  profileImage: {
    width: 180,
    height: 350,
    borderRadius: 10,
    marginRight: 15,
    borderWidth: 2,
    borderColor: '#6B4EFF',
  },
  infoContainer: {
    flex: 1,
  },
  label: {
    fontSize: 18,
    fontWeight: 'bold',
    marginBottom: 5,
    color: '#6B4EFF',
  },
  inputContainer: {
    borderColor: '#DDD',
    borderWidth: 1,
    borderRadius: 5,
    padding: 8,
    marginBottom: 15,
    backgroundColor: '#FFF',
  },
  input: {
    fontSize: 16,
    color: '#333',
  },
  buttonsContainer: {
    alignItems: 'center',
    marginTop: 15,
    width: '100%',
  },
  button: {
    backgroundColor: '#6B4EFF',
    padding: 12,
    borderRadius: 10,
    marginVertical: 8,
    width: '75%',
    alignItems: 'center',
    elevation: 2,
    flexDirection: 'row',
    justifyContent: 'center',
  },
  buttonText: {
    color: '#FFFFFF',
    fontSize: 18,
    letterSpacing: 0.5,
    marginLeft: 5,
  },
  logoutButton: {
    backgroundColor: '#FF6B6B',
    padding: 12,
    borderRadius: 10,
    marginVertical: 8,
    width: '75%',
    alignItems: 'center',
    elevation: 2,
    flexDirection: 'row',
    justifyContent: 'center',
  },
  logoutButtonText: {
    color: '#FFFFFF',
    fontSize: 18,
    letterSpacing: 0.5,
    marginLeft: 5,
  },
  icon: {
    marginRight: 5,
  },
  modalContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: 'rgba(0, 0, 0, 0.5)',
  },
  modalContent: {
    backgroundColor: '#fff',
    padding: 20,
    borderRadius: 10,
    width: '80%',
    alignItems: 'center',
  },
  modalTitle: {
    fontSize: 20,
    fontWeight: 'bold',
    marginBottom: 20,
  },
  modalInput: {
    width: '100%',
    borderColor: '#ddd',
    borderWidth: 1,
    borderRadius: 5,
    paddingHorizontal: 10,
    marginBottom: 10,
    fontSize: 16,
  },
  modalButton: {
    backgroundColor: '#6B4EFF',
    padding: 12,
    borderRadius: 10,
    marginVertical: 5,
    width: '100%',
    alignItems: 'center',
  },
  modalButtonText: {
    color: '#fff',
    fontSize: 16,
  },
});
