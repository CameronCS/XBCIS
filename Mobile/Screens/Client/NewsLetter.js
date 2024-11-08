import React, { useEffect, useState } from 'react';
import { View, Text, StyleSheet, FlatList, Image, TouchableOpacity, Modal, Alert } from 'react-native';
import axios from 'axios';
import { API_URL, GET_NEWSLETTERS } from '../Shared/Link'; // Adjust import based on your project structure

export default function NewsletterPage() {
  const [newsletters, setNewsletters] = useState([]);
  const [modalVisible, setModalVisible] = useState(false);
  const [selectedItem, setSelectedItem] = useState(null);

  useEffect(() => {
    // Fetch newsletters from the server
    axios.get(GET_NEWSLETTERS)
      .then(response => {
        setNewsletters(response.data.results);
        console.log(JSON.stringify(newsletters));
        
      })
      .catch(error => {
        console.error('Error fetching newsletters:', error);
        Alert.alert("Error", "Could not fetch newsletters.");
      });
  }, []);

  const handlePress = (item) => {
    setSelectedItem(item);
    setModalVisible(true);
  };

  return (
    <View style={styles.container}>
      <FlatList
        data={newsletters}
        keyExtractor={item => item.id.toString()}
        renderItem={({ item }) => (
          <TouchableOpacity style={styles.itemContainer} onPress={() => handlePress(item)}>
            <Image source={{ uri: `${API_URL}${item.image_path}` }} style={styles.image} />
            <Text style={styles.title}>{item.title}</Text>
            <Text style={styles.date}>{item.date}</Text>
          </TouchableOpacity>
        )}
      />
      <Modal
        animationType="slide"
        transparent={true}
        visible={modalVisible}
        onRequestClose={() => setModalVisible(false)}
      >
        <View style={styles.modalView}>
          <TouchableOpacity style={styles.closeButton} onPress={() => setModalVisible(false)}>
            <Text style={styles.closeButtonText}>✕</Text>
          </TouchableOpacity>
          {selectedItem && (
            <View style={styles.modalContent}>
              <Image source={{ uri: `${API_URL}${selectedItem.image_path}` }} style={styles.modalImage} />
              <Text style={styles.modalTitle}>{selectedItem.title}</Text>
              <Text style={styles.modalDescription}>{selectedItem.description}</Text>
            </View>
          )}
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
  itemContainer: {
    padding: 10,
    borderBottomWidth: 1,
    borderBottomColor: '#EEE',
    alignItems: 'center',
  },
  image: {
    width: 200,
    height: 100,
    resizeMode: 'cover',
  },
  title: {
    fontSize: 20,
    fontWeight: 'bold',
    color: '#333',
  },
  date: {
    fontSize: 14,
    color: '#999',
  },
  modalView: {
    margin: 20,
    backgroundColor: 'white',
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
  modalContent: {
    alignItems: 'center',
  },
  modalImage: {
    width: 200,
    height: 200,
    resizeMode: 'contain',
  },
  modalTitle: {
    fontSize: 24,
    fontWeight: 'bold',
    marginVertical: 10,
  },
  modalDescription: {
    fontSize: 16,
    textAlign: 'center',
  },
  closeButton: {
    position: 'absolute',
    top: 10,
    right: 10,
  },
  closeButtonText: {
    fontSize: 24,
    color: '#000',
  },
});
