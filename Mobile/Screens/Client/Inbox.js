import React, { useState, useEffect } from 'react';
import { View, Text, ScrollView, StyleSheet, Image, Modal, TouchableOpacity, Alert } from 'react-native';
import axios from 'axios'; // Import axios for API requests
import { GET_EMAIL } from '../Shared/Link';

export default function Inbox({ currentUser }) {
  const [modalVisible, setModalVisible] = useState(false);
  const [selectedMessage, setSelectedMessage] = useState("");
  const [messages, setMessages] = useState([]);

  useEffect(() => {
    // Function to fetch messages from the server
    const fetchMessages = () => {
      axios.get(`${GET_EMAIL}?receiver_username=${currentUser.username}`)
        .then(response => {
          console.log(JSON.stringify(response.data))
          const { emails } = response.data 
          setMessages(emails);
        })
        .catch(error => {
          console.error('Error fetching messages:', error);
          Alert.alert("Error", "Unable to fetch messages");
        });
    };

    // Fetch messages initially when the component mounts
    fetchMessages();

    // Set up a 10-second interval to re-fetch messages
    const intervalId = setInterval(fetchMessages, 10000); // 10 seconds = 10000 ms

    // Clear the interval when the component unmounts
    return () => clearInterval(intervalId);
  }, [currentUser.username]);  // Re-run the effect when the username changes

  const handlePress = (message) => {
    setSelectedMessage(message);
    setModalVisible(true);
  };

  return (
    <View style={styles.container}>
      <ScrollView style={styles.scrollContainer}>
        {messages.length === 0 ? (
          <Text style={styles.noMessagesText}>No messages available.</Text>
        ) : (
          messages.map((msg, index) => (
            <TouchableOpacity key={index} style={styles.item} onPress={() => handlePress(msg.body)}>
              <Image 
                source={{ uri: "https://icons.veryicon.com/png/o/internet--web/prejudice/user-128.png" }} 
                style={styles.icon} 
              />
              <View style={styles.textContainer}>
                <Text style={styles.itemTitle}>{msg.sender_username}</Text>
                <Text style={styles.msgTitle}>{msg.subject}</Text>
                <Text style={styles.itemPreview}>{msg.body}</Text>
              </View>
            </TouchableOpacity>
          ))
        )}
      </ScrollView>
      <Modal
        animationType="slide"
        transparent={true}
        visible={modalVisible}
        onRequestClose={() => setModalVisible(!modalVisible)}
      >
        <View style={styles.centeredView}>
          <View style={styles.modalView}>
            <Text style={styles.modalText}>{selectedMessage}</Text>
            <TouchableOpacity
              style={styles.button}
              onPress={() => setModalVisible(!modalVisible)}
            >
              <Text style={styles.buttonText}>Close</Text>
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
  },
  scrollContainer: {
    padding: 20,
  },
  item: {
    flexDirection: 'row',
    backgroundColor: '#FFFFFF',
    borderRadius: 20,
    padding: 15,
    marginBottom: 10,
    alignItems: 'center',
  },
  icon: {
    width: 64,
    height: 64,
    borderRadius: 32,
    marginRight: 10,
  },
  textContainer: {
    flex: 1,
  },
  itemTitle: {
    fontSize: 18,
    fontWeight: 'bold',
  },
  itemPreview: {
    fontSize: 14,
  },
  msgTitle: {
    fontSize: 16,
    fontWeight: 'bold',
  },
  centeredView: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    marginTop: 22,
  },
  modalView: {
    margin: 20,
    backgroundColor: '#FFFFFF',
    borderRadius: 20,
    padding: 35,
    alignItems: 'center',
    shadowColor: '#000',
    shadowOffset: {
      width: 0,
      height: 2,
    },
    shadowOpacity: 0.25,
    shadowRadius: 4,
    elevation: 5,
  },
  modalText: {
    marginBottom: 15,
    textAlign: 'center',
  },
  button: {
    borderRadius: 20,
    padding: 10,
    elevation: 2,
    backgroundColor: '#FF69B4',
  },
  buttonText: {
    color: 'white',
    fontWeight: 'bold',
    textAlign: 'center',
  },
  noMessagesText: {
    fontSize: 18,
    color: '#FFFFFF',
    textAlign: 'center',
    marginTop: 20,
  },
});
