import React, { useState, useEffect } from 'react';
import { View, Text, FlatList, TouchableOpacity, Alert, StyleSheet, Image, Modal } from 'react-native';
import * as ImagePicker from 'expo-image-picker';
import AsyncStorage from '@react-native-async-storage/async-storage';
import * as FileSystem from 'expo-file-system';
import { Ionicons } from '@expo/vector-icons';

export default function AdminGalleryManagement() {
  const [gallery, setGallery] = useState({
    sport: Array(20).fill(null),
    art: Array(20).fill(null),
    outings: Array(20).fill(null),
  });
  const [selectedCategory, setSelectedCategory] = useState('sport');
  const [selectedImage, setSelectedImage] = useState(null);
  const [isImageModalVisible, setIsImageModalVisible] = useState(false);

  // Load gallery from AsyncStorage on mount
  useEffect(() => {
    const loadGallery = async () => {
      const storedGallery = await AsyncStorage.getItem('gallery');
      if (storedGallery) {
        setGallery(JSON.parse(storedGallery));
      }
    };
    loadGallery();
  }, []);

  // Save gallery to AsyncStorage
  const saveGallery = async (updatedGallery) => {
    await AsyncStorage.setItem('gallery', JSON.stringify(updatedGallery));
  };

  const addImage = async () => {
    const permissionResult = await ImagePicker.requestMediaLibraryPermissionsAsync();
    if (!permissionResult.granted) {
      Alert.alert('Permission required', 'Permission to access the gallery is required!');
      return;
    }

    const pickerResult = await ImagePicker.launchImageLibraryAsync();
    if (!pickerResult.canceled) {
      const availableSlotIndex = gallery[selectedCategory].indexOf(null);
      if (availableSlotIndex !== -1) {
        const fileName = pickerResult.uri.split('/').pop();
        const newFileUri = `${FileSystem.documentDirectory}${fileName}`;
        await FileSystem.copyAsync({ from: pickerResult.uri, to: newFileUri });

        const updatedCategoryImages = [...gallery[selectedCategory]];
        updatedCategoryImages[availableSlotIndex] = { id: Date.now().toString(), uri: newFileUri };

        const updatedGallery = { ...gallery, [selectedCategory]: updatedCategoryImages };
        setGallery(updatedGallery);
        saveGallery(updatedGallery);
      } else {
        Alert.alert('Category Full', 'No available slots in this category.');
      }
    }
  };

  const deleteImage = (category, index) => {
    Alert.alert('Confirm Deletion', 'Are you sure you want to delete this image?', [
      { text: 'Cancel', style: 'cancel' },
      {
        text: 'Delete',
        onPress: async () => {
          const updatedCategoryImages = [...gallery[category]];
          const fileToDelete = updatedCategoryImages[index]?.uri;
          updatedCategoryImages[index] = null;

          const updatedGallery = { ...gallery, [category]: updatedCategoryImages };
          setGallery(updatedGallery);
          saveGallery(updatedGallery);

          if (fileToDelete) {
            await FileSystem.deleteAsync(fileToDelete);
          }
        },
      },
    ]);
  };

  const openImageModal = (image) => {
    setSelectedImage(image);
    setIsImageModalVisible(true);
  };

  const renderImage = ({ item, index }) => (
    item ? (
      <TouchableOpacity style={styles.imageContainer} onPress={() => openImageModal(item.uri)}>
        <Image source={{ uri: item.uri }} style={styles.image} />
        <TouchableOpacity
          onPress={() => deleteImage(selectedCategory, index)}
          style={styles.deleteButton}>
          <Ionicons name="trash-outline" size={20} color="#fff" />
        </TouchableOpacity>
      </TouchableOpacity>
    ) : (
      <View style={styles.emptySlot} />
    )
  );

  return (
    <View style={styles.container}>
      <Text style={styles.header}>Admin Gallery Management</Text>

      <View style={styles.categorySelector}>
        <TouchableOpacity
          style={[styles.categoryButton, selectedCategory === 'sport' ? styles.activeCategory : null]}
          onPress={() => setSelectedCategory('sport')}>
          <Text style={styles.categoryButtonText}>Sport</Text>
        </TouchableOpacity>
        <TouchableOpacity
          style={[styles.categoryButton, selectedCategory === 'art' ? styles.activeCategory : null]}
          onPress={() => setSelectedCategory('art')}>
          <Text style={styles.categoryButtonText}>Art</Text>
        </TouchableOpacity>
        <TouchableOpacity
          style={[styles.categoryButton, selectedCategory === 'outings' ? styles.activeCategory : null]}
          onPress={() => setSelectedCategory('outings')}>
          <Text style={styles.categoryButtonText}>Outings</Text>
        </TouchableOpacity>
      </View>

      <TouchableOpacity style={styles.addButton} onPress={addImage}>
        <Ionicons name="add-circle-outline" size={24} color="#fff" />
        <Text style={styles.addButtonText}>Add Image</Text>
      </TouchableOpacity>

      <FlatList
        data={gallery[selectedCategory]}
        renderItem={renderImage}
        numColumns={4}
        keyExtractor={(item, index) => index.toString()}
        style={styles.imageList}
      />

      {/* Modal to show the image in full view */}
      {selectedImage && (
        <Modal visible={isImageModalVisible} transparent={true} onRequestClose={() => setIsImageModalVisible(false)}>
          <View style={styles.modalContainer}>
            <TouchableOpacity style={styles.closeModalButton} onPress={() => setIsImageModalVisible(false)}>
              <Ionicons name="close-circle-outline" size={36} color="#fff" />
            </TouchableOpacity>
            <Image source={{ uri: selectedImage }} style={styles.fullImage} />
          </View>
        </Modal>
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 16,
    backgroundColor: '#F6A1F8',
  },
  header: {
    fontSize: 26,
    fontWeight: 'bold',
    marginBottom: 20,
    textAlign: 'center',
    color: '#fff',
  },
  categorySelector: {
    flexDirection: 'row',
    justifyContent: 'space-around',
    marginBottom: 20,
  },
  categoryButton: {
    padding: 10,
    borderRadius: 10,
    backgroundColor: '#6B4EFF',
    width: 100,
    alignItems: 'center',
  },
  activeCategory: {
    backgroundColor: '#ff4444',
  },
  categoryButtonText: {
    color: '#fff',
    fontWeight: 'bold',
  },
  addButton: {
    backgroundColor: '#6B4EFF',
    padding: 15,
    borderRadius: 10,
    alignItems: 'center',
    flexDirection: 'row',
    justifyContent: 'center',
    marginBottom: 20,
  },
  addButtonText: {
    color: '#fff',
    fontWeight: 'bold',
    fontSize: 18,
    marginLeft: 10,
  },
  imageList: {
    marginTop: 16,
  },
  imageContainer: {
    width: 80,
    height: 80,
    margin: 5,
    position: 'relative',
    borderRadius: 10,
    overflow: 'hidden',
    borderWidth: 2,
    borderColor: '#fff',
  },
  image: {
    width: '100%',
    height: '100%',
  },
  deleteButton: {
    position: 'absolute',
    top: 5,
    right: 5,
    backgroundColor: '#ff4444',
    padding: 5,
    borderRadius: 5,
  },
  emptySlot: {
    width: 80,
    height: 80,
    backgroundColor: '#ddd',
    margin: 5,
    borderRadius: 10,
  },
  modalContainer: {
    flex: 1,
    backgroundColor: 'rgba(0,0,0,0.8)',
    justifyContent: 'center',
    alignItems: 'center',
  },
  closeModalButton: {
    position: 'absolute',
    top: 40,
    right: 20,
  },
  fullImage: {
    width: '90%',
    height: '80%',
    resizeMode: 'contain',
  },
});
