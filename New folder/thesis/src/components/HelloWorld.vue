<script setup>
import { ref } from 'vue'

defineProps({
  title: String,
  image: String,
  description: String,
})

const count = ref(0)
</script>

<template>
  <div class="bg-gray-200 flex items-center justify-center mb-10 pb-10">
    <div class="container mx-auto p-10 pb-10 bg-gray-200" id="start">
      <h3 class="text-5xl text-center mx-auto mb-4 pb-2 font-extrabold tracking-tight pb-10 relative text-gray-800">School projects<span class="borderedText"></span></h3>
      <div class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-4 gap-1">
        <!-- Replace this with your grid items -->
      
          <img :src="image" :alt="image" class="w-full h-48 rounded-md object-cover">
          <div class="px-1 py-4">
            <div class="font-bold text-xl mb-2">{{ title }}</div>
            <p class="text-gray-700 text-base">{{ description }}</p>
          </div>
        <!-- Add more items as needed -->
      </div>
    </div>
  </div>
</template>

<style scoped>

</style>
