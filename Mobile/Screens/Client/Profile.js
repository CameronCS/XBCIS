import React, { useEffect, useState } from 'react';
import { View, Text, TextInput, Image, Button, StyleSheet, Alert } from 'react-native';
import { Picker } from '@react-native-picker/picker';
import axios from 'axios';
import { API_URL, GET_CHILD } from '../Shared/Link';

export default function Profile({ navigation, currentUser }) {
  const [selectedProfile, setSelectedProfile] = useState('user');
  const [profileData, setProfileData] = useState(currentUser);
  const [children, setChildren] = useState([]);

  const handleProfileChange = (itemValue) => {
    if (itemValue === 'user') {
      setProfileData(currentUser);
    } else {
      const selectedChild = children.find(child => child.first_name === itemValue);
      setProfileData(selectedChild);
    }
    setSelectedProfile(itemValue);
  };

  const handleLogout = () => {
    navigation.navigate("LoginPage");
  };

  useEffect(() => {
    console.log(JSON.stringify(currentUser))
    axios.get(`${GET_CHILD}?username=${currentUser.username}`)
      .then(response => {
        setChildren(response.data.children); // Save the kids data in state
      })
      .catch(error => {
        console.error('Error fetching kids data:', error);
        Alert.alert("Error", "Unable to fetch kids data");
      });
  }, []);

  console.log('Current Profile Data:', profileData);
  console.log('Profile Picture URL:', profileData.image_path);

  return (
    <View style={styles.container}>
      <Text style={styles.profileText}>Profile</Text>
      <View style={styles.profileContainer}>
        <Picker
          onValueChange={handleProfileChange}
          selectedValue={selectedProfile}
          style={styles.picker}
        >
          <Picker.Item label={`${currentUser.first_name} (You)`} value='user' />
          {children.map(child => (
            <Picker.Item key={child.id} label={child.first_name} value={child.first_name} />
          ))}
        </Picker>

        <Image
          source={{
            uri: selectedProfile === 'user'
              ? `${API_URL}${profileData.image_path}`
              : `${API_URL}${profileData.image_path}`
          }}
          style={styles.profileImage}
        />

        <View style={styles.detailsContainer}>
          <Text style={styles.label}>First Name:</Text>
          <TextInput
            value={profileData.first_name}
            editable={false}
            style={[styles.textInput, styles.readOnlyInput]}
          />
          <Text style={styles.label}>Last Name:</Text>
          <TextInput
            value={profileData.last_name}
            editable={false}
            style={[styles.textInput, styles.readOnlyInput]}
          />
          <Text style={styles.label}>{selectedProfile === 'user' ? 'Username' : 'Class Name'}:</Text>
          <TextInput
            value={selectedProfile === 'user' ? profileData.username : profileData.class_name}
            editable={false}
            style={[styles.textInput, styles.readOnlyInput]}
          />
          <Text style={styles.label}>Contact Number:</Text>
          <TextInput
            value={currentUser.contact_no}
            editable={false}
            style={[styles.textInput, styles.readOnlyInput]}
          />
        </View>
      </View>
      <View style={styles.buttonContainer}>
        <Button title='Log Out' onPress={handleLogout} color="#FF69B4" />
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#F6A1F8',
    alignItems: 'center',
    justifyContent: 'center',
  },
  profileText: {
    fontSize: 24,
    fontWeight: 'bold',
    marginBottom: 20,
    color: '#FFFFFF',
  },
  picker: {
    height: 50,
    width: 300,
    marginBottom: 20,
  },
  profileContainer: {
    alignItems: 'center',
    marginBottom: 30,
  },
  profileImage: {
    width: 100,
    height: 100,
    borderRadius: 50,
    marginBottom: 20,
    borderWidth: 2,
    borderColor: '#FFFFFF',
  },
  detailsContainer: {
    backgroundColor: '#FFFFFF',
    borderRadius: 10,
    padding: 20,
    width: 300,
    shadowColor: "#000",
    shadowOffset: {
      width: 0,
      height: 2,
    },
    shadowOpacity: 0.25,
    shadowRadius: 3.84,
    elevation: 5,
  },
  label: {
    fontSize: 16,
    marginBottom: 5,
    color: '#333333',
  },
  textInput: {
    height: 40,
    borderColor: '#CCCCCC',
    borderWidth: 1,
    borderRadius: 5,
    marginBottom: 15,
    paddingHorizontal: 10,
    backgroundColor: '#F8F8F8',
  },
  readOnlyInput: {
    color: '#000000',
    backgroundColor: '#FFFFFF',
  },
  buttonContainer: {
    marginTop: 20,
  }
});
