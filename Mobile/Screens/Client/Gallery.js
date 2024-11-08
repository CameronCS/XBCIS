import React, { useEffect, useState } from 'react';
import { View, Text, StyleSheet, ScrollView, TouchableOpacity, Modal, Image, Alert } from 'react-native';
import axios from 'axios';
import * as FileSystem from 'expo-file-system';
import * as MediaLibrary from 'expo-media-library';
import { API_URL, GET_GALLERY_EVENTS, GET_GALLERY_IMAGES, GET_GALLERY_YEARS } from '../Shared/Link';

export default function GalleryPage() {
  const [events, setEvents] = useState([]);
  const [years, setYears] = useState([]);
  const [selectedYear, setSelectedYear] = useState(null);
  const [selectedEvent, setSelectedEvent] = useState(null);
  const [images, setImages] = useState([]);
  const [modalVisible, setModalVisible] = useState(false);

  useEffect(() => {
    // Fetch available years
    axios.get(GET_GALLERY_YEARS)
      .then(response => {
        setYears(response.data.results);
      })
      .catch(error => {
        console.error('Error fetching gallery years:', error);
      });
  }, []);

  const handleYearSelect = (year) => {
    if (selectedYear === year) {
      setSelectedYear(null); // Deselect the year if it is already selected
      setEvents([]); // Clear events since no year is selected
    } else {
      setSelectedYear(year);
      // Fetch event names for the selected year
      let url = `${GET_GALLERY_EVENTS}${year}`;
      axios.get(url)
        .then(response => {
          setEvents(response.data.events);
        })
        .catch(error => {
          console.error('Error fetching gallery events:', error);
        });
    }
  };
  
  const handleEventPress = (eventName) => {
    // Fetch images for the selected event and year
    let url = `${GET_GALLERY_IMAGES}${selectedYear}`;
    axios.post(url, { event_name: eventName })
      .then(response => {
        setImages(response.data.images);
        setSelectedEvent(eventName);
        setModalVisible(true);
      })
      .catch(error => {
        console.error('Error fetching gallery images:', error);
      });
  };

  const handleDownload = async (imageUri) => {
    try {
      const { uri } = await FileSystem.downloadAsync(
        imageUri,
        FileSystem.documentDirectory + imageUri.split('/').pop()
      );
      const asset = await MediaLibrary.createAssetAsync(uri);
      await MediaLibrary.createAlbumAsync('Download', asset, false);
      Alert.alert('Download complete', 'Image saved to your gallery.');
    } catch (error) {
      console.error('Error downloading or saving image:', error);
      Alert.alert('Error', 'Failed to download or save image.');
    }
  };

  return (
    <View style={styles.container}>
      <ScrollView contentContainerStyle={styles.contentContainer}>
        <Text style={styles.headerText}>Select a Year</Text>
        {years.map((year, index) => (
          <TouchableOpacity 
            key={index} 
            style={selectedYear === year ? styles.selectedYearContainer : styles.yearContainer} 
            onPress={() => handleYearSelect(year)}
          >
            <Text style={styles.yearText}>{year}</Text>
          </TouchableOpacity>
        ))}
        
        {selectedYear && (
          <>
            <Text style={styles.headerText}>Select an Event</Text>
            {events.map((event, index) => (
              <TouchableOpacity 
                key={index} 
                style={styles.eventContainer} 
                onPress={() => handleEventPress(event)}
              >
                <Text style={styles.eventText}>{event}</Text>
              </TouchableOpacity>
            ))}
          </>
        )}
      </ScrollView>

      <Modal
        animationType="slide"
        transparent={true}
        visible={modalVisible}
        onRequestClose={() => setModalVisible(false)}
      >
        <View style={styles.modalView}>
          <Text style={styles.modalTitle}>{selectedEvent}</Text>
          <ScrollView style={styles.modalContent}>
            {images.map((image, index) => (
              <View key={index} style={styles.imageContainer}>
                <Image
                  source={{ uri: `${API_URL}${image.image_path[0] === '/' ? image.image_path : `/${image.image_path}`}` }}
                  style={styles.modalImage}
                />
                <TouchableOpacity
                  style={styles.downloadButton}
                  onPress={() => handleDownload(`${API_URL}${image.image_path}`)}
                >
                  <Text style={styles.downloadButtonText}>Download</Text>
                </TouchableOpacity>
              </View>
            ))}
          </ScrollView>
          <TouchableOpacity style={styles.closeButton} onPress={() => setModalVisible(false)}>
            <Text style={styles.closeButtonText}>Close</Text>
          </TouchableOpacity>
        </View>
      </Modal>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#F6A1F8',
    paddingHorizontal: 10,
    paddingTop: 20,
  },
  contentContainer: {
    flexDirection: 'column',
    alignItems: 'center',
    justifyContent: 'center',
  },
  headerText: {
    fontSize: 22,
    fontWeight: 'bold',
    color: '#333',
    marginVertical: 15,
  },
  yearContainer: {
    margin: 10,
    padding: 20,
    backgroundColor: '#FFFFFF',
    borderRadius: 10,
    borderColor: '#FF69B4',
    borderWidth: 2,
    width: '80%',
    alignItems: 'center',
  },
  selectedYearContainer: {
    margin: 10,
    padding: 20,
    backgroundColor: '#FF69B4',
    borderRadius: 10,
    borderColor: '#FF69B4',
    borderWidth: 2,
    width: '80%',
    alignItems: 'center',
  },
  yearText: {
    fontSize: 18,
    color: '#333',
  },
  eventContainer: {
    margin: 10,
    padding: 20,
    backgroundColor: '#FFFFFF',
    borderRadius: 10,
    borderColor: '#FF69B4',
    borderWidth: 2,
    width: '80%',
    alignItems: 'center',
  },
  eventText: {
    fontSize: 18,
    color: '#333',
  },
  modalView: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: 'rgba(0,0,0,0.7)',
    padding: 20,
  },
  modalTitle: {
    fontSize: 24,
    fontWeight: 'bold',
    color: '#FFFFFF',
    marginBottom: 10,
  },
  modalContent: {
    flex: 1,
    width: '100%',
  },
  imageContainer: {
    marginBottom: 20,
    alignItems: 'center',
  },
  modalImage: {
    width: '100%',
    height: 200,
    borderRadius: 10,
  },
  downloadButton: {
    backgroundColor: '#FF69B4',
    padding: 10,
    borderRadius: 5,
    marginTop: 10,
  },
  downloadButtonText: {
    color: '#FFFFFF',
    fontWeight: 'bold',
  },
  closeButton: {
    backgroundColor: '#FF69B4',
    padding: 10,
    borderRadius: 5,
    marginTop: 20,
  },
  closeButtonText: {
    color: '#FFFFFF',
    fontWeight: 'bold',
  },
});
